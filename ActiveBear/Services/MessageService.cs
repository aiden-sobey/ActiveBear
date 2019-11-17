using System;
using System.Threading.Tasks;
using ActiveBear.Hubs;
using ActiveBear.Models;
using Newtonsoft.Json;

namespace ActiveBear.Services
{
    public class MessageService : BaseService
    {
        public MessageService(ActiveBearContext _context) : base(_context) { }

        public async Task<Message> NewMessage(User sender, Channel channel, string content)
        {
            if (sender == null || channel.Id == Guid.Empty || string.IsNullOrEmpty(content))
                return null;

            var newMessage = new Message
            {
                Sender = sender.Name,
                Channel = channel.Id,
                EncryptedContents = content
            };

            context.Add(newMessage);
            await context.SaveChangesAsync();

            return newMessage;
        }

        public async Task<Message> NewMessageFromPacket(string messagePacket)
        {
            Guid channelId, userCookie;
            MessagePacket packet;

            try
            {
                packet = JsonConvert.DeserializeObject<MessagePacket>(messagePacket);
                channelId = packet.Channel;
                userCookie = packet.UserCookie;
            }
            catch
            {
                return null;
            }

            var channelService = new ChannelService(context);
            var userService = new UserService(context);
            var channelAuthService = new ChannelAuthService(context);

            var channel = await channelService.GetChannel(channelId);
            var user = await userService.ExistingUser(userCookie);
            var auth = await channelAuthService.UserIsAuthed(channel, user);
            if (!auth || channel == null || user == null)
                return null;

            return await NewMessage(user, channel, packet.Message);
        }
    }
}
