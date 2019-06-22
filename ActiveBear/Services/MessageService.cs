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

        public static Message NewMessageFromPacket(string messagePacket)
        {
            var context = DbService.NewDbContext();
            var decodedMessage = JsonConvert.DeserializeObject<MessagePacket>(messagePacket);

            var channel = context.Channels.FirstOrDefault(c => c.Id.ToString() == decodedMessage.Channel);
            var user = context.Users.FirstOrDefault(u => u.CookieId.ToString() == decodedMessage.UserCookie);
            if (channel == null || user == null) return new Message();

            return NewMessage(user.Name, channel.Id, decodedMessage.Message);
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
