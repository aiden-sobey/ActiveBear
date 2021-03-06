﻿using ActiveBear.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ActiveBear.Services
{
    public class ChannelAuthService : BaseService
    {
        public ChannelAuthService(ActiveBearContext _context) : base(_context) { }

        public async Task<bool> CreateAuth(Channel channel, User user)
        {
            if (channel == null || user == null) return false;
            if (await UserIsAuthed(channel, user)) return false;

            var channelAuth = new ChannelAuth
            {
                User = user.Name,
                Channel = channel.Id,
                HashedKey = channel.KeyHash
            };

            context.Add(channelAuth);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UserIsAuthed(Channel channel, User user)
        {
            if (channel == null || user == null)
                return false;

            var auth = await context.ChannelAuths.FirstOrDefaultAsync(au => au.Channel == channel.Id &&
                                                                            au.User == user.Name);

            return auth != null;
        }
    }
}
