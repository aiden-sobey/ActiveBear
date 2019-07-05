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
            var userCookie = JsonConvert.DeserializeObject<CookiePacket>(cookiePacket).UserCookie;
            var currentUser = await UserService.ExistingUser(Guid.Parse(userCookie));
            if (currentUser == null) return;

            // We just want to send the name, not the whole user object
            await Clients.Caller.SendAsync("CurrentUser", currentUser.Name);
        }

        // Client has sent us a message
        public async Task SendMessage(string messagePacket)
        {
            if (string.IsNullOrEmpty(messagePacket)) return;

            // Create a message from the serialized packet
            var message = await MessageService.NewMessageFromPacket(messagePacket);
            var messageBlob = JsonConvert.SerializeObject(message);
            await Clients.Group(ChatHubHelper.GroupFor(messagePacket)).SendAsync
                ("ReceiveMessage", messageBlob);
        }

        // Client has requested all messages for this channel
        public async Task GetChannelMessages(string channelInfoPacket)
        {
            var messages = await ChannelService.MessagesFor(channelInfoPacket);
            var channelMessages = JsonConvert.SerializeObject(messages);
            await Clients.Caller.SendAsync("ReceiveAllMessages", channelMessages);

            // Add the user to the relevant signalR group
            await Groups.AddToGroupAsync(Context.ConnectionId, ChatHubHelper.GroupFor(channelInfoPacket));
            // Notify group of new member
            var userCookie = JsonConvert.DeserializeObject<ChannelInfoPacket>(channelInfoPacket).UserCookie;
            var currentUser = await UserService.ExistingUser(Guid.Parse(userCookie));
            var notification = currentUser.Name + " has joined!";

            await Clients.Group(ChatHubHelper.GroupFor(channelInfoPacket)).SendAsync
                ("Notification", notification);
        }

        // Attempt to create a new channel from the given data
        public async Task CreateChannel(string channelCreationPacket)
        {
            Channel channel = await ChannelService.CreateChannel(channelCreationPacket);

            if (channel != null)
                await Clients.Caller.SendAsync("ChannelCreated", channel.Id);
        }
    }
}
