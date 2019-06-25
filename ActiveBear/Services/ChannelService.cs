﻿using System;
using ActiveBear.Models;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using ActiveBear.Hubs;

namespace ActiveBear.Services
{
    public static class ChannelService
    {
        public static Channel CreateChannel(string title, string key, User createUser)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(key) || createUser == null)
                return null;

            var context = DbService.NewDbContext();
            var channel = new Channel(context)
            {
                Title = title,
                KeyHash = key,
                CreateUser = createUser.Name
            };

            context.Add(channel);
            context.SaveChanges();

            return channel;
        }

        public static Channel CreateChannel(string channelCreationPacket)
        {
            var packet = JsonConvert.DeserializeObject<ChannelCreationPacket>(channelCreationPacket);

            if (packet.ChannelKey == null   || packet.ChannelKey == string.Empty ||
                packet.UserCookie == null   || packet.UserCookie == string.Empty ||
                packet.ChannelTitle == null || packet.ChannelTitle == string.Empty)
            {
                return null;
            }

            var user = CookieService.CurrentUser(packet.UserCookie);
            return CreateChannel(packet.ChannelTitle, packet.ChannelKey, user);
        }

        public static List<Message> MessagesFor(Channel channel)
        {
            var context = DbService.NewDbContext();
            return context.Messages.Where(m => m.Channel == channel.Id).ToList();
        }

        public static List<Message> MessagesFor(string channelInfoPacket)
        {
            var context = DbService.NewDbContext();
            var decodedPacket = JsonConvert.DeserializeObject<ChannelInfoPacket>(channelInfoPacket);

            var channel = context.Channels.FirstOrDefault(c => c.Id.ToString() == decodedPacket.Channel);
            var user = context.Users.FirstOrDefault(u => u.CookieId.ToString() == decodedPacket.UserCookie);
            var auth = ChannelAuthService.UserIsAuthed(channel, user);

            if (auth)
                return context.Messages.Where(m => m.Channel == channel.Id).ToList();

            return new List<Message>();
        }
    }
}
