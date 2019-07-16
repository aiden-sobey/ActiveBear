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
        private Channel channel;
        private User user;
        private MessagePacket packet;

        private const string Lorem = "Lorem";

        [SetUp]
        protected async Task SetUp()
        {
            user = await UserService.CreateUser(Lorem, Lorem, Lorem);
            channel = await ChannelService.CreateChannel(Lorem, Lorem, user);
            packet = new MessagePacket
            {
                UserCookie = user.CookieId.ToString(),
                Channel = channel.Id.ToString(),
                Message = Lorem
            };
        }

        [Test]
        public async Task NewMessageCreatesMessage()
        {
            var message = await MessageService.NewMessage(user, channel, Lorem);
            Assert.Equals(Lorem, message.EncryptedContents);
            await CheckMessageExists(message);
        }

        [Test]
        public async Task InvalidNewMessageReturnsNull()
        {
            Assert.IsNull(await MessageService.NewMessage(null, channel, Lorem));
            Assert.IsNull(await MessageService.NewMessage(user, null, Lorem));
            Assert.IsNull(await MessageService.NewMessage(user, channel, ""));

            Assert.IsNull(await NewMessageFromPacket(string.Empty, channel.Id.ToString(), Lorem));
            Assert.IsNull(await NewMessageFromPacket(user.CookieId.ToString(), string.Empty, Lorem));
            Assert.IsNull(await NewMessageFromPacket(string.Empty, channel.Id.ToString(), Lorem));
            Assert.IsNull(await NewMessageFromPacket(user.CookieId.ToString(), channel.Id.ToString(), string.Empty));
        }

        [Test]
        public async Task NewMessageFromPacketCreatesMessage()
        {
            var jsonPacket = JsonConvert.SerializeObject(packet);
            var message = await MessageService.NewMessageFromPacket(jsonPacket);
            await CheckMessageExists(message);
        }

        // Helpers

        private async Task<Message> NewMessageFromPacket(string userCookie, string channelId, string message)
        {
            var messagePacket = new MessagePacket
            {
                UserCookie = userCookie,
                Channel = channelId,
                Message = message
            };

            var jsonPacket = JsonConvert.SerializeObject(messagePacket);
            return await MessageService.NewMessageFromPacket(jsonPacket);
        }

        private async Task CheckMessageExists(Message message)
        {
            Assert.IsNotNull(message);
            var channelMessages = await ChannelService.MessagesFor(message.Channel);
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
