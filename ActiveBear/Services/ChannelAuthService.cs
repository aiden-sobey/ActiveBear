using System;
using System.Linq;
using System.Collections.Generic;
using ActiveBear.Models;

namespace ActiveBear.Services
{
    public static class ChannelAuthService
    {
        public static void CreateAuth(Channel channel, User user, ActiveBearContext context)
        {
            var channelAuth = new ChannelAuth
            {
                User = user.Name,
                Channel = channel.Id,
                HashedKey = channel.KeyHash
            };

            context.Add(channelAuth);
            context.SaveChanges();
        }

        public static bool UserIsAuthed(Channel channel, User user, ActiveBearContext context)
        {
            if (UsersAuthedFor(channel, context).Contains(user))
                return true;
            else
                return false;
        }

        private static List<User> UsersAuthedFor(Channel channel, ActiveBearContext context)
        {
            // Get relevant auths
            var channelAuths = context.ChannelAuths.Where(a => a.Channel == channel.Id).ToList();
            var authedUserNames = channelAuths.Select(c => c.User).ToList();
            var authedUsers = new List<User>();

            foreach(var name in authedUserNames)
            {
                var user = context.Users.Where(u => u.Name == name).FirstOrDefault();
                authedUsers.Add(user);
            }

            return authedUsers;
        }
    }
}
