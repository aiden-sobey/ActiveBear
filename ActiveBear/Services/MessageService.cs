using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveBear.Hubs;
using ActiveBear.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ActiveBear.Services
{
    public static class MessageService
    {
        public static async Task<Message> NewMessage(User sender, Channel channel, string content)
        {
            if (sender == null || channel.Id == Guid.Empty || string.IsNullOrEmpty(content))
                return null;

            var context = DbService.NewDbContext();

            var newMessage = new Message
            {
                Sender = sender.Name,
                Channel = channel.Id,
                EncryptedContents = content
            };

            context.Add(newMessage);
            await context.SaveChangesAsync();

            return newMessage;
        }

        public static async Task<Message> NewMessageFromPacket(string messagePacket)
        {
            Guid channelId, userCookie;
            MessagePacket packet;

            try
            {
                packet = JsonConvert.DeserializeObject<MessagePacket>(messagePacket);
                channelId = Guid.Parse(packet.Channel);
                userCookie = Guid.Parse(packet.UserCookie);
            }
            catch
            {
                return null;
            }

            var channel = await ChannelService.GetChannel(channelId);
            var user = await UserService.ExistingUser(userCookie);
            var auth = await ChannelAuthService.UserIsAuthed(channel, user);
            if (!auth || channel == null || user == null)
                return null;

            return await NewMessage(user, channel, packet.Message);
        }
    }
}
