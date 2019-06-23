using System;
using ActiveBear.Models;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using ActiveBear.Hubs;

namespace ActiveBear.Services
{
    public static class ChannelService
    {
        public static Channel CreateChannel(string title, string rawKey, User createUser)
        {
            var errors = new List<String>();

            if (String.IsNullOrEmpty(title))
                errors.Add("Title was empty!!!");

            if (String.IsNullOrEmpty(rawKey))
                errors.Add("rawKey was empty!!!");

            var context = DbService.NewDbContext();
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

        public static List<Message> MessagesFor(Channel channel)
        {
            var context = DbService.NewDbContext();
            return context.Messages.Where(m => m.Channel == channel.Id).ToList();
        }

        public static List<Message> MessagesFor(string channelPacket)
        {
            var messages = new List<Message>();
            var context = DbService.NewDbContext();
            var decodedPacket = JsonConvert.DeserializeObject<ChannelPacket>(channelPacket);

            var channel = context.Channels.FirstOrDefault(c => c.Id.ToString() == decodedPacket.Channel);
            var user = context.Users.FirstOrDefault(u => u.CookieId.ToString() == decodedPacket.UserCookie);
            var auth = ChannelAuthService.UserIsAuthed(channel, user);

            if (auth)
                messages = context.Messages.Where(m => m.Channel == channel.Id).ToList();

            return messages;
        }
    }
}
