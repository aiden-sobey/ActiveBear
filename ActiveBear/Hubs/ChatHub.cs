using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using ActiveBear.Services;
using ActiveBear.Models;
using Newtonsoft.Json;
using System;

namespace ActiveBear.Hubs
{
    public class ChatHub : Hub
    {
        // Client requests the current user (lookup by cookieID)
        public async Task CurrentUser(string cookiePacket)
        {
            if (string.IsNullOrEmpty(cookiePacket)) return;

            try
            {
                // Handling direct user input here, so be very cautious with it
                var userCookie = JsonConvert.DeserializeObject<CookiePacket>(cookiePacket).UserCookie;
                var currentUser = await UserService.ExistingUser(Guid.Parse(userCookie));
                if (currentUser != null)
                    await Clients.Caller.SendAsync("CurrentUser", currentUser.Name);
            }
            catch
            {
                return; // Invalid userCookie within the cookiePacket
            }
        }

        // Client has sent us a message
        public async Task SendMessage(string messagePacket)
        {
            if (string.IsNullOrEmpty(messagePacket)) return;

            var message = await MessageService.NewMessageFromPacket(messagePacket);

            if (message == null) return;    // If our message creation failed

            var messageBlob = JsonConvert.SerializeObject(message);
            await Clients.Group(ChatHubHelper.GroupFor(messagePacket)).SendAsync
                ("ReceiveMessage", messageBlob);
        }

        // Client has requested all messages for this channel
        public async Task GetChannelMessages(string channelInfoPacket)
        {
            ChannelInfoPacket packet;
            User currentUser;
            Channel channel;

            try
            {
                packet = JsonConvert.DeserializeObject<ChannelInfoPacket>(channelInfoPacket);
                currentUser = await UserService.ExistingUser(Guid.Parse(packet.UserCookie));
                channel = await ChannelService.GetChannel(Guid.Parse(packet.Channel));
            }
            catch
            {
                return; // Error parsing the packet
            }

            if (currentUser == null) return;

            // Send channel messages to the validated user
            var messages = await ChannelService.MessagesFor(channel);
            var channelMessages = JsonConvert.SerializeObject(messages);
            await Clients.Caller.SendAsync("ReceiveAllMessages", channelMessages);

            // Add the user to the relevant signalR group
            await Groups.AddToGroupAsync(Context.ConnectionId, ChatHubHelper.GroupFor(channelInfoPacket));
            // Notify group they joined
            var notification = currentUser.Name + " has joined!";
            await Clients.Group(ChatHubHelper.GroupFor(channelInfoPacket)).SendAsync("Notification", notification);
        }

        // Attempt to create a new channel from the given data
        public async Task CreateChannel(string channelCreationPacket)
        {
            try
            {
                var packet = JsonConvert.DeserializeObject<ChannelCreationPacket>(channelCreationPacket);
                var user = await UserService.ExistingUser(packet.UserCookie);
                Channel channel = await ChannelService.CreateChannel(
                    packet.ChannelTitle, packet.ChannelKey, user);

                if (channel != null) await Clients.Caller.SendAsync("ChannelCreated", channel.Id);
            }
            catch
            {
                return;
            }
        }
    }
}
