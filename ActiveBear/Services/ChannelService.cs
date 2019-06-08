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
                KeyHash = HashKey(rawKey),
                CreateUser = CurrentUser(context).Name
            };

            rawKey = String.Empty;

            context.Add(channel);
            context.SaveChanges();

            return channel;
        }

        // TODO: Make this some global context-y thing
        private static User CurrentUser(ActiveBearContext context)
        {
            return context.Users.FirstOrDefault();
        }

        private static string HashKey(string rawKey)
        {
            // TODO SHA-1 Hash here
            return String.Empty;
        }
    }
}
