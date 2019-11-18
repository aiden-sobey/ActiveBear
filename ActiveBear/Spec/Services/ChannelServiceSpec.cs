using System;
using System.Threading.Tasks;
using ActiveBear.Models;
using ActiveBear.Services;
using NUnit.Framework;

namespace ActiveBear.Spec.Services
{
    [TestFixture]
    public class ChannelServiceSpec
    {
        private User user;

        private ActiveBearContext context;
        private UserService userService;
        private ChannelService channelService;
        private MessageService messageService;

        private const string Lorem = "Lorem";
        private const int MessageCount = 5;

        [SetUp]
        protected async Task SetUp()
        {
            context = DbService.NewTestContext();
            userService = new UserService(context);
            channelService = new ChannelService(context);
            messageService = new MessageService(context);

            user = await userService.ExistingUser(Lorem);
            if (user == null)
                user = await userService.CreateUser(Lorem, Lorem, Lorem);
        }

        [TearDown]
        protected void TearDown()
        {
            context.Dispose();
        }

        // Create Channel

        [Test]
        public async Task CreateChannelFromIncompleteDataFails()
        {
            Assert.IsNull(await channelService.CreateChannel(Lorem, "", user));
            Assert.IsNull(await channelService.CreateChannel(null, Lorem, user));
            Assert.IsNull(await channelService.CreateChannel(Lorem, Lorem, null));
            Assert.IsNull(await channelService.CreateChannel(null, null, null));
        }

        [Test]
        public async Task CreateChannelFromValidDataPasses()
        {
            var channel = await channelService.CreateChannel(Lorem, Lorem, user);
            var hashedPassword = EncryptionService.Sha256(Lorem);
            Assert.AreEqual(user.Name, channel.CreateUser);
            Assert.AreEqual(Lorem, channel.Title);
            // TODO: move all password hashing server side
            //Assert.AreEqual(hashedPassword, channel.KeyHash);
        }

        [Test]
        public async Task DbSaveErrorIsHandled()
        {
            // Try to save the same thing twice, or somehow force a SaveChanges error
            var channel = await channelService.CreateChannel(Lorem, Lorem, user);
            Assert.IsNotNull(channel);

            var copyChannel = new Channel
            {
                Title = Lorem,
                KeyHash = Lorem
            };
            
            // Make this an illegitimate channel then try to save it
            copyChannel.Id = channel.Id;
            Assert.Throws<InvalidOperationException>(() => { context.Add(copyChannel); });
        }

        // MessagesFor

        [Test]
        public async Task MessagesForNullChannelIsEmpty()
        {
            var objectMessages = await channelService.MessagesFor(channel: null);
            Assert.IsEmpty(objectMessages);
            Assert.IsNotNull(objectMessages);
        }
        
        [Test]
        public async Task MessagesForEmptyChannelIsEmpty()
        {
            var messages = await channelService.MessagesFor(new Channel());
           Assert.IsEmpty(messages);
        }

        [Test]
        public async Task MessagesForPopulatedChannelSucceeds()
        {
            var channel = new Channel();
            Assert.IsNotNull(channel);
            await PopulateWithMessages(channel);
            
            var messages = await channelService.MessagesFor(channel);

            Assert.IsNotNull(messages);
            Assert.AreEqual(MessageCount, messages.Count);
        }

        // Helpers

        private async Task PopulateWithMessages(Channel channel)
        {
            for (int i = 0; i < MessageCount; i++)
                Assert.IsNotNull(await messageService.NewMessage(user, channel, Lorem));
        }
    }
}
