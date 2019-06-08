using System;
using System.Linq;
using System.Collections.Generic;
using ActiveBear.Models;

namespace ActiveBear.Services
{
    public static class ChannelAuthService
    {
        // TODO: make this work
        public static List<User> AuthedUsersFor(Channel channel, ActiveBearContext context)
        {
            return context.Users.Where(x => 1 == 1).ToList();
        }
    }
}
