using ActiveBear.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace ActiveBear.Services
{
    public class ChannelService : BaseService
    {
        public ChannelService(ActiveBearContext _context) : base(_context) { }

        public async Task<Channel> CreateChannel(string title, string key, User createUser)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(key) || createUser == null)
                return null;

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

        public async Task<List<Message>> MessagesFor(Channel channel)
        {
            if (channel == null) return new List<Message>();
            return await context.Messages.Where(m => m.Channel == channel.Id).ToListAsync();
        }

        public async Task<List<Message>> MessagesFor(Guid channelId)
        {
            if (channelId == Guid.Empty) return new List<Message>();

            return await MessagesFor(await GetChannel(channelId));
        }

        // TODO: write test coverage for this
        public async Task<Channel> GetChannel(Guid channelId)
        {
            return await context.Channels.FirstOrDefaultAsync(c => c.Id == channelId);
        }
    }
}
