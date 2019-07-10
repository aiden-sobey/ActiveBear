using ActiveBear.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ActiveBear.Services
{
    public static class ChannelAuthService
    {
        public static async Task CreateAuth(Channel channel, User user)
        {
            if (channel == null || user == null) return;
            if (await UserIsAuthed(channel, user)) return;

            var context = DbService.NewDbContext();

            var channelAuth = new ChannelAuth
            {
                User = user.Name,
                Channel = channel.Id,
                HashedKey = channel.KeyHash
            };

            context.Add(channelAuth);
            await context.SaveChangesAsync();
        }

        public static async Task<bool> UserIsAuthed(Channel channel, User user)
        {
            if (channel == null || user == null)
                return false;

            var context = DbService.NewDbContext();
            var auth = await context.ChannelAuths.FirstOrDefaultAsync(au => au.Channel == channel.Id &&
                                                                            au.User == user.Name);

            return auth != null;
        }
    }
}
