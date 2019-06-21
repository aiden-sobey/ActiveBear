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

        public static List<Message> MessagesFor(string channelPacket)
        {
            // TODO: dont hardcode
            const string hardcodedGuid = "34c34235-84a1-40fa-840d-19f6ede9a8cc";
            var context = DbService.NewDbContext();

            return context.Messages.Where(m => m.Channel == Guid.Parse(hardcodedGuid)).ToList();
        }
    }
}
