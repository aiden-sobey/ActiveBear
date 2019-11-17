using System;
using System.Linq;
using System.Threading.Tasks;
using ActiveBear.Hubs;
using ActiveBear.Models;
using ActiveBear.Services;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ActiveBear.Spec.Services
{
    [TestFixture]
    public class MessageServiceSpec
    {
        private User user;
        private Channel channel;

        private ActiveBearContext context;
        private UserService userService;
        private ChannelService channelService;
        private MessageService messageService;

        private const string Lorem = "Lorem";
        private string packet;

        [SetUp]
        protected async Task SetUp()
        {
            context = DbService.NewDbContext();
            userService = new UserService(context);
            channelService = new ChannelService(context);
            messageService = new MessageService(context);

            user = await userService.CreateUser(Lorem, Lorem, Lorem);
            channel = await channelService.CreateChannel(Lorem, Lorem, user);

            packet = JsonConvert.SerializeObject(new MessagePacket
            {
                UserCookie = user.CookieId,
                Channel = channel.Id,
                Message = Lorem
            });
        }

        [TearDown]
        protected void TearDown()
        {
            context.Dispose();
        }

        [Test]
        public async Task NewMessageCreatesMessage()
        {
            var message = await messageService.NewMessage(user, channel, Lorem);
            Assert.Equals(Lorem, message.EncryptedContents);
            await CheckMessageExists(message);
        }

        [Test]
        public async Task NewMessageFromPacketCreatesMessage()
        {
            var message = await messageService.NewMessageFromPacket(packet);
            await CheckMessageExists(message);
        }

        [Test]
        public async Task InvalidNewMessageReturnsNull()
        {
            Assert.IsNull(await messageService.NewMessage(null, channel, Lorem));
            Assert.IsNull(await messageService.NewMessage(user, null, Lorem));
            Assert.IsNull(await messageService.NewMessage(user, channel, ""));

            Assert.IsNull(await NewMessageFromPacket(Guid.Empty, channel.Id, Lorem));
            Assert.IsNull(await NewMessageFromPacket(user.CookieId, Guid.Empty, Lorem));
            Assert.IsNull(await NewMessageFromPacket(Guid.Empty, channel.Id, Lorem));
            Assert.IsNull(await NewMessageFromPacket(user.CookieId, channel.Id, string.Empty));
        }

        // Helpers

        private async Task<Message> NewMessageFromPacket(Guid userCookie, Guid channelId, string message)
        {
            var messagePacket = new MessagePacket
            {
                UserCookie = userCookie,
                Channel = channelId,
                Message = message
            };

            var jsonPacket = JsonConvert.SerializeObject(messagePacket);
            return await messageService.NewMessageFromPacket(jsonPacket);
        }

        private async Task CheckMessageExists(Message message)
        {
            Assert.IsNotNull(message);
            var channelMessages = await channelService.MessagesFor(message.Channel);
            Assert.IsNotEmpty(channelMessages);
            var savedMessage = channelMessages.FirstOrDefault(m => m.Id == message.Id);
            Assert.IsNotNull(savedMessage);

            Assert.Equals(Lorem, message.EncryptedContents);
            Assert.Equals(message.EncryptedContents, savedMessage.EncryptedContents);
            Assert.Equals(message.Channel, savedMessage.Channel);
            Assert.Equals(message.Sender, savedMessage.Sender);
            Assert.Equals(message.SendDate, savedMessage.SendDate);
        }
    }
}
