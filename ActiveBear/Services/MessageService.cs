﻿using System;
using System.Collections.Generic;
using System.Linq;
using ActiveBear.Models;

namespace ActiveBear.Services
{
    public static class MessageService
    {
        public static Message NewMessage(User sender, Channel channel, string encryptedContents, ActiveBearContext context)
        {
            var newMessage = new Message
            {
                Sender = sender.Name,
                Channel = channel.Id,
                EncryptedContents = encryptedContents
            };

            context.Add(newMessage);
            context.SaveChanges();

            return newMessage;
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
}
