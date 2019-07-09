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
            var context = DbService.NewDbContext();
            var decodedMessage = JsonConvert.DeserializeObject<MessagePacket>(messagePacket);

            var channel = await context.Channels.FirstOrDefaultAsync(c => c.Id.ToString() == decodedMessage.Channel);
            var user = await context.Users.FirstOrDefaultAsync(u => u.CookieId.ToString() == decodedMessage.UserCookie);
            var auth = await ChannelAuthService.UserIsAuthed(channel, user);
            if (!auth || channel == null || user == null)
                return new Message();

            var message = await NewMessage(user, channel, decodedMessage.Message);
            return message;
        }

        public static async Task<List<Message>> ChannelMessages(Channel channel)
        {
            var context = DbService.NewDbContext();
            var messages = await context.Messages.Where(m => m.Channel == channel.Id).ToListAsync();

            return messages;
        }

        public static async Task<Dictionary<Message, User>> LinkMessagesToUsers(List<Message> messages, ActiveBearContext context)
        {
            var link = new Dictionary<Message, User>();

            foreach (var message in messages)
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.Name == message.Sender);
                if (user != null)
                    link.Add(message, user);
            }

            return link;
        }
    }
}
