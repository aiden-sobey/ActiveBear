using System;
using System.Linq;
using System.Collections.Generic;
using ActiveBear.Models;

namespace ActiveBear.Services
{
    public static class ChannelAuthService
    {
        public static void CreateAuth(Channel channel, User user)
        {
            var context = DbService.NewDbContext();

            var channelAuth = new ChannelAuth
            {
                User = user.CookieId,
                Channel = channel.Id,
                HashedKey = channel.KeyHash
            };

            context.Add(channelAuth);
            context.SaveChanges();
        }

        public static bool UserIsAuthed(Channel channel, User user)
        {
            if (channel == null || user == null)
                return false;

            var context = DbService.NewDbContext();
            var auth = context.ChannelAuths.FirstOrDefault(au => au.Channel == channel.Id && au.User == user.CookieId);

            return auth != null;
        }
    }
}
