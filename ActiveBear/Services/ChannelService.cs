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
        public static Channel CreateChannel(string title, string rawKey)
        {
            var errors = new List<String>();

            if (String.IsNullOrEmpty(title))
                errors.Add("Title was empty!!!");

            if (String.IsNullOrEmpty(rawKey))
                errors.Add("rawKey was empty!!!");

            var channel = new Channel
            {
                Id = Guid.NewGuid(),
                Title = title,
                MemberCount = 1,
                AuthorisedUsers = new List<User> { CurrentUser() },
                Messages = new List<Message>(),
                Status = "ACTIVE",
                KeyHash = HashKey(rawKey),
                CreateDate = DateTime.Now,
                CreateUser = CurrentUser()
            };

            rawKey = String.Empty;

            // TODO: actually create the channel here - i.e. save to DB

            return channel;
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
