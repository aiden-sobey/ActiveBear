using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ActiveBear.Models;
using Newtonsoft.Json;

namespace ActiveBear.Services
{
    public static class MessageService
    {
        public static Message NewMessage(string senderName, Guid channelId, string content)
        {
            if (string.IsNullOrEmpty(senderName) || channelId == Guid.Empty || string.IsNullOrEmpty(content))
                return null;

            var context = DbService.NewDbContext();

            var newMessage = new Message
            {
                Sender = senderName,
                Channel = channelId,
                EncryptedContents = content
            };

            context.Add(newMessage);
            context.SaveChanges();

            return newMessage;
        }

        public static string NewMessageFromPacket(string messagePacket)
        {
            // Deserialize messagePacket
            var context = DbService.NewDbContext();

            var message = JsonConvert.DeserializeObject<MessagePacket>(messagePacket);
            var channel = context.Channels.FirstOrDefault(c => c.Id.ToString() == message.Channel);
            var user = context.Users.FirstOrDefault(u => u.CookieId.ToString() == message.UserCookie);
            if (channel == null || user == null)
                return string.Empty;

            var generatedMessage = NewMessage(user.Name, channel.Id, message.Message);

            return generatedMessage.EncryptedContents;
            // Construct message from packet
        }

        public static List<Message> ChannelMessages(Channel channel, ActiveBearContext context)
        {
            var messages = context.Messages.Where(m => m.Channel == channel.Id).ToList();
            var key = channel.KeyHash;

            return messages;
        }

        public static Dictionary<Message, User> LinkMessagesToUsers(List<Message> messages, ActiveBearContext context)
        {
            var link = new Dictionary<Message, User>();

            foreach (var message in messages)
            {
                var user = context.Users.Where(u => u.Name == message.Sender).FirstOrDefault();
                if (user != null)
                    link.Add(message, user);
            }

            return link;
        }
    }

    [DataContract]
    class MessagePacket
    {
        [DataMember]
        public string UserCookie = string.Empty;

        [DataMember]
        public string Channel = string.Empty;

        [DataMember]
        public string Message = string.Empty;
    }
}
