using System.Threading.Tasks;
using ActiveBear.Models;
using ActiveBear.Services;
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
            await context.SaveChangesAsync();
        }

        // MessagesFor

        [Test]
        public async Task MessagesForNullChannelIsEmpty()
        {
            var objectMessages = await ChannelService.MessagesFor(channel: null);
            Assert.IsEmpty(objectMessages);
            Assert.IsNotNull(objectMessages);
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

        // Helpers

        private async Task PopulateWithMessages(Channel channel)
        {
            for (int i = 0; i < MessageCount; i++)
                Assert.IsNotNull(await MessageService.NewMessage(user, channel, Lorem));
        }
    }
}
