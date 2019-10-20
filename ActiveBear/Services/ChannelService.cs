using ActiveBear.Models;
using System.Collections.Generic;
using System.Linq;
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
            await context.SaveChangesAsync();

            return channel;
        }

        public static async Task<List<Message>> MessagesFor(Channel channel)
        {
            if (channel == null) return new List<Message>();
            var context = DbService.NewDbContext();
            return await context.Messages.Where(m => m.Channel == channel.Id).ToListAsync();
        }
        
        // TODO: write test coverage for this
        public static async Task<Channel> GetChannel(Guid channelId)
        {
            var context = DbService.NewDbContext();
            return await context.Channels.FirstOrDefaultAsync(c => c.Id == channelId);
        }

        // TODO: tests for this
        public static async Task<bool> DeleteChannel(Guid channelId)
        {
            var context = DbService.NewDbContext();

            var channel = await GetChannel(channelId);
            if (channel == null) return true; // TODO: should this be false?

            var channelAuths = await ChannelAuthService.ChannelAuthsFor(channel);
            var messages = await MessagesFor(channel);

            context.Channels.Remove(channel);
            context.ChannelAuths.RemoveRange(channelAuths);
            context.Messages.RemoveRange(messages);

            var save = await context.SaveChangesAsync();
            return save == 0;
        }
    }
}
