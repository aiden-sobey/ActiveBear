using System;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using ActiveBear.Services;

namespace ActiveBear.Hubs
{
    public class ChatHub : Hub
    {
        // Client has sent us a message
        // TODO: it needs to be encrypted before this point
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
        public async Task GetChannelMessages(string channelPacket)
        {
			var channelMessages = ChannelService.MessagesFor(channelPacket);

            // Quick hack
            foreach (var message in channelMessages)
			{
				await Clients.Caller.SendAsync("ReceiveMessage", message.EncryptedContents);
			}

			//await Clients.Caller.SendAsync("ReceiveAllMessages", channelMessages);
        }
    }
}
