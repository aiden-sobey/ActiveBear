using System;
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
        public static Channel CreateChannel(string title, string key, User createUser)
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
            context.SaveChanges();

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
            return CreateChannel(packet.ChannelTitle, packet.ChannelKey, user);
        }

        public static List<Message> MessagesFor(Channel channel)
        {
            var context = DbService.NewDbContext();
            return context.Messages.Where(m => m.Channel == channel.Id).ToList();
        }

        public static async Task<List<Message>> MessagesForAsync(string channelInfoPacket)
        {
            var context = DbService.NewDbContext();
            var decodedPacket = JsonConvert.DeserializeObject<ChannelInfoPacket>(channelInfoPacket);

            var channel = await context.Channels.FirstOrDefaultAsync(c => c.Id.ToString() == decodedPacket.Channel);
            var user = await context.Users.FirstOrDefaultAsync(u => u.CookieId.ToString() == decodedPacket.UserCookie);
            var auth = await ChannelAuthService.UserIsAuthed(channel, user);
            if (auth)
                return context.Messages.Where(m => m.Channel == channel.Id).ToList();

            return new List<Message>();
        }
    }
}
