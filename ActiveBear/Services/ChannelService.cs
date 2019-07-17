using ActiveBear.Models;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using ActiveBear.Hubs;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace ActiveBear.Services
{
    public static class ChannelService
    {
        public static async Task<Channel> CreateChannel(string title, string key, User createUser)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(key) || createUser == null)
                return null;

            var context = DbService.NewDbContext();
            var channel = new Channel
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
            User user;

            try
            {
                packet = JsonConvert.DeserializeObject<ChannelCreationPacket>(channelCreationPacket);
                user = await UserService.ExistingUser(Guid.Parse(packet.UserCookie));
                return await CreateChannel(packet.ChannelTitle, packet.ChannelKey, user);
            }
            catch
            {
                return null;    // Deserialization error
            }
        }

        public static async Task<List<Message>> MessagesFor(Channel channel)
        {
            if (channel == null) return new List<Message>();
            var context = DbService.NewDbContext();
            return await context.Messages.Where(m => m.Channel == channel.Id).ToListAsync();
        }

        public static async Task<List<Message>> MessagesFor(Guid channelId)
        {
            if (channelId == Guid.Empty) return new List<Message>();

            return await MessagesFor(await GetChannel(channelId));
        }

        // TODO: write test coverage for this
        public static async Task<Channel> GetChannel(Guid channelId)
        {
            var context = DbService.NewDbContext();
            return await context.Channels.FirstOrDefaultAsync(c => c.Id == channelId);
        }
    }
}
