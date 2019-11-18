using System;
using System.Linq;
using System.Threading.Tasks;
using ActiveBear.Hubs;
using ActiveBear.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ActiveBear.Spec.Services
{
    [TestFixture]
    public class MessageServiceSpec : ServiceSpec
    {
        [Test]
        public async Task NewMessageCreatesMessage()
        {
            var message = await messageService.NewMessage(user, channel, Lorem);
            Assert.AreEqual(Lorem, message.EncryptedContents);
            await CheckMessageExists(message);
        }

        [Test]
        public async Task NewMessageFromPacketCreatesMessage()
        {
            await authService.CreateAuth(channel, user);
            var message = await messageService.NewMessageFromPacket(packet);
            await CheckMessageExists(message);
        }

        [Test]
        public async Task NewMessageFromPacketWithoutAuthFails()

        {
            var message = await messageService.NewMessageFromPacket(packet);
            Assert.IsNull(message);
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

            Assert.AreEqual(Lorem, message.EncryptedContents);
            Assert.AreEqual(message.EncryptedContents, savedMessage.EncryptedContents);
            Assert.AreEqual(message.Channel, savedMessage.Channel);
            Assert.AreEqual(message.Sender, savedMessage.Sender);
            Assert.AreEqual(message.SendDate, savedMessage.SendDate);
        }
    }
}
