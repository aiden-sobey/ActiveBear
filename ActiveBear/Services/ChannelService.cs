using System;
using ActiveBear.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace ActiveBear.Services
{
    public static class ChannelService
    {
        public static Channel CreateChannel(string title, string rawKey, ActiveBearContext context, User createUser)
        {
            var errors = new List<String>();

            if (String.IsNullOrEmpty(title))
                errors.Add("Title was empty!!!");

            if (String.IsNullOrEmpty(rawKey))
                errors.Add("rawKey was empty!!!");

            var channel = new Channel(context)
            {
                Title = title,
                KeyHash = rawKey, //TODO some encrytion here
                CreateUser = createUser.Name
            };

            rawKey = String.Empty;

            context.Add(channel);
            context.SaveChanges();

            return channel;
        }

        public static List<Message> MessagesFor(Channel channel, ActiveBearContext context)
        {
            return context.Messages.Where(m => m.Channel == channel.Id).ToList();
        }
    }
}
