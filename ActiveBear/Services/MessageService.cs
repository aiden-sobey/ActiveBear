using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ActiveBear.Models;

namespace ActiveBear.Services
{
    public static class MessageService
    {
        public static Message NewMessage(string senderName, Guid channelId, string encryptedContents)
        {
            if (string.IsNullOrEmpty(senderName) || channelId == Guid.Empty || string.IsNullOrEmpty(encryptedContents))
                return null;

            var context = DbService.NewDbContext();

            var newMessage = new Message
            {
                Sender = senderName,
                Channel = channelId,
                EncryptedContents = encryptedContents
            };

            context.Add(newMessage);
            context.SaveChanges();

            return newMessage;
        }

        public static Message NewMessageFromPacket(string messagePacket)
        {
            // Deserialize messagePacket

            // Construct message from packet

            return new Message();
        }

        public static List<Message> ChannelMessages(Channel channel, ActiveBearContext context)
        {
            var messages = context.Messages.Where(m => m.Channel == channel.Id).ToList();
            var key = channel.KeyHash;

            // TODO: Is this encryption pointless if the key is stored in the DB...
            foreach (var message in messages)
            {
                message.EncryptedContents = EncryptionService.AesDecrypt(message.EncryptedContents, key);
            }

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
        public string UserCookie;

        [DataMember]
        public string Channel;

        [DataMember]
        public string Message;
    }
}
