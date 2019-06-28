using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using ActiveBear.Services;
using ActiveBear.Models;
using Newtonsoft.Json;

namespace ActiveBear.Hubs
{
    public class ChatHub : Hub
    {
        // Client has sent us a message
        public async Task SendMessage(string messagePacket)
        {
            if (string.IsNullOrEmpty(messagePacket)) return;

            // Create a message from the serialized packet
            var message = MessageService.NewMessageFromPacket(messagePacket);

            await Clients.Group(ChatHubHelper.GroupFor(messagePacket)).SendAsync
                ("ReceiveMessage", message.EncryptedContents);
        }

        // Client has requested all messages for this channel
        public async Task GetChannelMessages(string channelInfoPacket)
        {
            // Group membership automatically expires on connection loss
            await Groups.AddToGroupAsync(Context.ConnectionId, ChatHubHelper.GroupFor(channelInfoPacket));
            var channelMessages = JsonConvert.SerializeObject(ChannelService.MessagesFor(channelInfoPacket));

            await Clients.Caller.SendAsync("ReceiveAllMessages", channelMessages);

        }

        // Attempt to create a new channel from the given data
        public async Task CreateChannel(string channelCreationPacket)
        {
            Channel channel = ChannelService.CreateChannel(channelCreationPacket);

            if (channel != null)
                await Clients.Caller.SendAsync("ChannelCreated", channel.Id);
        }
    }
}
