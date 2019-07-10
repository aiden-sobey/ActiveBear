using System;
using System.Threading.Tasks;
using ActiveBear.Hubs;
using ActiveBear.Models;
using ActiveBear.Services;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ActiveBear.Spec.Services
{
    [TestFixture]
    public class ChannelServiceSpec
    {
        private const int MessageCount = 5;
        private const string Lorem = "Lorem";

        private User user;

        [SetUp]
        protected async Task SetUp()
        {
            user = await UserService.CreateUser(Lorem, Lorem, Lorem);
        }

        // Create Channel

        [Test]
        public async Task CreateChannelFromIncompleteDataFails()
        {
            Assert.IsNull(await ChannelService.CreateChannel(Lorem, "", user));
            Assert.IsNull(await ChannelService.CreateChannel(null, Lorem, user));
            Assert.IsNull(await ChannelService.CreateChannel(Lorem, Lorem, null));
            Assert.IsNull(await ChannelService.CreateChannel(null, null, null));
        }

        [Test]
        public async Task CreateChannelFromValidDataPasses()
        {

            var channel = await ChannelService.CreateChannel(Lorem, Lorem, user);
            Assert.AreEqual(user.Name, channel.CreateUser);
            Assert.AreEqual(Lorem, channel.Title);
            Assert.AreEqual(EncryptionService.Sha256(Lorem), channel.KeyHash);
        }

        [Test]
        public async Task DbSaveErrorIsHandled()
        {
            // Try to save the same thing twice, or somehow force a SaveChanges error
            var context = DbService.NewDbContext();
            var channel = await ChannelService.CreateChannel(Lorem, Lorem, user);

            var copyChannel = new Channel
            {
                Title = Lorem,
                KeyHash = Lorem
            };

            // Make this an illegitimate channel then try to save it
            copyChannel.Id = channel.Id;
            context.Add(copyChannel);
            _ = await context.SaveChangesAsync();
        }

        [Test]
        public async Task CreateChannelFromIncompletePacketFails()
        {
            string channelPacket;

            channelPacket = NewChannelCreationPacket(Lorem, string.Empty);
            Assert.IsNull(await ChannelService.CreateChannel(channelPacket));

            channelPacket = NewChannelCreationPacket(string.Empty, Lorem);
            Assert.IsNull(await ChannelService.CreateChannel(channelPacket));

            // TODO: fix this
            //channelPacket = NewChannelCreationPacket(Lorem, Lorem, Guid.Empty); // Todo this wont get properly tested
            Assert.IsNull(await ChannelService.CreateChannel(channelPacket));

            channelPacket = NewChannelCreationPacket(Lorem, null);
            Assert.IsNull(await ChannelService.CreateChannel(channelPacket));
        }

        [Test]
        public async Task CreateChannelFromInvalidPacketFails()
        {
            var channelPacket = "{Title:'Test', Key:'Test', CookieId:" + user.CookieId + "}";
            Assert.IsNull(await ChannelService.CreateChannel(channelPacket));

            Assert.IsNull(await ChannelService.CreateChannel(string.Empty));
            Assert.IsNull(await ChannelService.CreateChannel(JsonConvert.SerializeObject(user.CookieId)));
        }

        [Test]
        public async Task CreateChannelFromValidPacketSucceeds()
        {
            var channelPacket = NewChannelCreationPacket(Lorem, Lorem);
            var channel = await ChannelService.CreateChannel(channelPacket);
            Assert.AreEqual(channel.CreateUser, user.Name);
            Assert.AreEqual(channel.Title, Lorem);
            Assert.AreEqual(channel.KeyHash, EncryptionService.Sha256(Lorem));
        }

        // MessagesFor

        [Test]
        public async Task MessagesForNullChannelIsEmpty()
        {
            var objectMessages = await ChannelService.MessagesFor(channel: null);
            var packetMessages = await ChannelService.MessagesFor(channelInfoPacket: null);
            Assert.IsEmpty(objectMessages);
            Assert.IsEmpty(packetMessages);
            Assert.IsNotNull(objectMessages);
            Assert.IsNotNull(packetMessages);
        }
        
        [Test]
        public async Task MessagesForEmptyChannelIsEmpty()
        {
            var messages = await ChannelService.MessagesFor(new Channel());
           Assert.IsEmpty(messages);
        }

        [Test]
        public async Task MessagesForPopulatedChannelSucceeds()
        {
            var channel = new Channel();
            Assert.IsNotNull(channel);
            await PopulateWithMessages(channel);
            
            var messages = await ChannelService.MessagesFor(channel);

            Assert.IsNotNull(messages);
            Assert.AreEqual(MessageCount, messages.Count);
        }

        [Test]
        public async Task MessagesForValidPacketSucceeds()
        {
            var channel = await ChannelService.CreateChannel(Lorem, Lorem, user);
            Assert.IsNotNull(channel);
            await ChannelAuthService.CreateAuth(channel, user);
            Assert.IsTrue(await ChannelAuthService.UserIsAuthed(channel, user));
            await PopulateWithMessages(channel);

            var packet = NewChannelInfoPacket(channel);
            var messages = await ChannelService.MessagesFor(packet);

            Assert.AreEqual(MessageCount, messages.Count);
        }

        [Test]
        public async Task MessagesForInvalidPacketIsEmpty()
        {
            var channelPacket = "{Channel:'', CookieId:" + user.CookieId + "}";
            Assert.IsEmpty(await ChannelService.MessagesFor(channelPacket));
            await ChannelService.CreateChannel(null);
        }

        [Test]
        public async Task MessagesForUnAuthedUserIsEmpty()
        {
            var channel = await ChannelService.CreateChannel(Lorem, Lorem, user);
            Assert.IsNotNull(channel);
            await PopulateWithMessages(channel);

            var packet = NewChannelInfoPacket(channel);
            var messages = await ChannelService.MessagesFor(packet);

            Assert.IsEmpty(messages);
        }

        [Test]
        public async Task MessagesForInvalidUserIsEmpty()
        {
            var channel = await ChannelService.CreateChannel(Lorem, Lorem, user);
            Assert.IsNotNull(channel);
            await PopulateWithMessages(channel);

            var packetBlob = new ChannelInfoPacket
            {
                UserCookie = Guid.NewGuid().ToString(),
                Channel = channel.Id.ToString()
            };
            var packet = JsonConvert.SerializeObject(packetBlob);

            var messages = await ChannelService.MessagesFor(packet);
            Assert.IsEmpty(messages);
        }

        // Helpers

        private string NewChannelCreationPacket(string title=Lorem, string key=Lorem)
        {
            var channelObject = new ChannelCreationPacket
            {
                UserCookie = user.CookieId.ToString(),
                ChannelTitle = title,
                ChannelKey = key
            };

            return JsonConvert.SerializeObject(channelObject);
        }

        private string NewChannelInfoPacket(Channel channel)
        {
            var packet = new ChannelInfoPacket
            {
                UserCookie = user.CookieId.ToString(),
                Channel = channel.Id.ToString()
            };

            return JsonConvert.SerializeObject(packet);
        }

        private async Task PopulateWithMessages(Channel channel)
        {
            for (int i = 0; i < MessageCount; i++)
                Assert.IsNotNull(await MessageService.NewMessage(user, channel, Lorem));
        }
    }
}
