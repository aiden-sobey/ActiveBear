﻿using Microsoft.AspNetCore.SignalR;
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
