using ActiveBear.Models;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using ActiveBear.Hubs;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ActiveBear.Services
{
    public static class ChannelService
    {
        public static async Task<Channel> CreateChannel(string title, string key, User createUser)
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
            _ = await context.SaveChangesAsync();

            return channel;
        }

        public static async Task<Channel> CreateChannel(string channelCreationPacket)
        {
            ChannelCreationPacket packet;
            try
            {
                packet = JsonConvert.DeserializeObject<ChannelCreationPacket>(channelCreationPacket);
            }
            catch // Deserialization error
            {
                return null;
            }

            var user = await CookieService.CurrentUser(packet.UserCookie);
            return await CreateChannel(packet.ChannelTitle, packet.ChannelKey, user);
        }

        public static async Task<List<Message>> MessagesFor(Channel channel)
        {
            var context = DbService.NewDbContext();
            return await context.Messages.Where(m => m.Channel == channel.Id).ToListAsync();
        }

        public static async Task<List<Message>> MessagesFor(string channelInfoPacket)
        {
            var context = DbService.NewDbContext();
            var decodedPacket = JsonConvert.DeserializeObject<ChannelInfoPacket>(channelInfoPacket);

            var channel = await context.Channels.FirstOrDefaultAsync(c => c.Id.ToString() == decodedPacket.Channel);
            var user = await context.Users.FirstOrDefaultAsync(u => u.CookieId.ToString() == decodedPacket.UserCookie);
            var auth = await ChannelAuthService.UserIsAuthed(channel, user);
            if (auth)
                return await context.Messages.Where(m => m.Channel == channel.Id).ToListAsync();

            return new List<Message>();
        }
    }
}
