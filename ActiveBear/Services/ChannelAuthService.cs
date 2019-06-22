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

        private static List<User> UsersAuthedFor(Channel channel)
        {
            var context = DbService.NewDbContext();

            // Get relevant auths
            var channelAuths = context.ChannelAuths.Where(a => a.Channel == channel.Id).ToList();
            var authedCookies = channelAuths.Select(c => c.User).ToList();
            var authedUsers = new List<User>();

            foreach(var cookie in authedCookies)
            {
                var user = context.Users.Where(u => u.CookieId == cookie).FirstOrDefault();
                if (user != null)
                    authedUsers.Add(user);
            }

            return authedUsers;
        }
    }
}
