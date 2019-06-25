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

            // Send that method to (... all ...) clients
            // TODO: big security flaw here, check how to send it to only
            // members of the relevant channel
            await Clients.All.SendAsync("ReceiveMessage", message.EncryptedContents);
        }

        // Client has requested all messages for this channel
        public async Task GetChannelMessages(string channelInfoPacket)
        {
            var channelMessages = ChannelService.MessagesFor(channelInfoPacket);

            // TODO: batch this and just send one message packet
            foreach (var message in channelMessages)
            {
                await Clients.Caller.SendAsync("ReceiveMessage", message.EncryptedContents);
            }
        }

        public async Task CreateChannel(string channelCreationPacket)
        {
            Channel channel = ChannelService.CreateChannel(channelCreationPacket);

            if (channel == null)
                await Clients.Caller.SendAsync("BuildErrors", "Channel creation failed"); // TODO: implement this
            else
                await Clients.Caller.SendAsync("ChannelCreated", channel.Id); //TODO: implement this
        }
    }
}
