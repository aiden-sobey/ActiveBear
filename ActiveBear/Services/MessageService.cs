using System;
using ActiveBear.Models;

namespace ActiveBear.Services
{
    public static class MessageService
    {
        public static Message NewMessage(User sender, Channel channel, string encryptedContents, ActiveBearContext context)
        {
            var newMessage = new Message
            {
                Sender = sender.Id, //TODO: just ref sender instead of their guid
                Channel = channel.Id,
                EncryptedContents = encryptedContents
            };

            context.Add(newMessage);
            context.SaveChanges();

            return newMessage;
        }
    }
}
