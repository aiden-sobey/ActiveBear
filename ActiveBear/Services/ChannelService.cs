using System;
using ActiveBear.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ActiveBear.Services
{
    public static class ChannelService
    {
        public static Channel CreateChannel(string title, string rawKey, ActiveBearContext context)
        {
            var errors = new List<String>();

            if (String.IsNullOrEmpty(title))
                errors.Add("Title was empty!!!");

            if (String.IsNullOrEmpty(rawKey))
                errors.Add("rawKey was empty!!!");

            var channel = new Channel(context)
            {
                Title = title,
                AuthorisedUsers = new List<User> { CurrentUser() },
                KeyHash = HashKey(rawKey),
                CreateUser = CurrentUser(),
            };

            rawKey = String.Empty;

            context.Add(channel);
            context.SaveChanges();

            return channel;
        }

        public static Dictionary<Message, User> LinkMessagesToUsers(List<Message> messages, ActiveBearContext context)
        {
            var link = new Dictionary<Message, User>();

            foreach (var message in messages)
            {
                var user = context.Users.Where(u => u.Id == message.Sender).FirstOrDefault();
                if (user != null)
                    link.Add(message, user);
            }

            return link;
        }

        // TODO: Make this some global context-y thing
        private static User CurrentUser()
        {
            return new User();
        }

        private static string HashKey(string rawKey)
        {
            // TODO SHA-1 Hash here
            return String.Empty;
        }
    }
}
