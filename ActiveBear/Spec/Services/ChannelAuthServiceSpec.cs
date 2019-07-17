using System.Linq;
using System.Threading.Tasks;
using ActiveBear.Models;
using ActiveBear.Services;
using NUnit.Framework;

namespace ActiveBear.Spec.Services
{
    [TestFixture]
    public class ChannelAuthServiceSpec
    {
        private User user;
        private Channel channel;

        private const string Lorem = "Lorem";

        [SetUp]
        protected async Task SetUp()
        {
            user = await UserService.CreateUser(Lorem, Lorem, Lorem);
            channel = await ChannelService.CreateChannel(Lorem, Lorem, user);
        }

        // Create Auth

        [Test]
        public async Task CreateAuthForAlreadyAuthedUserExits()
        {
            var context = DbService.NewDbContext();

            // Create auth
            await ChannelAuthService.CreateAuth(channel, user);

            // Assert auth count is one
            var authCount = context.ChannelAuths.Where(ca => ca.User == user.Name).ToList().Count;
            Assert.AreEqual(1, authCount);

            // Try create second auth
            await ChannelAuthService.CreateAuth(channel, user);

            // Asser auth count is still one
            authCount = context.ChannelAuths.Where(ca => ca.User == user.Name).ToList().Count;
            Assert.AreEqual(1, authCount);
        }

        [Test]
        public async Task CreateAuthWithEmptyParamsExits()
        {
            Assert.IsFalse(await ChannelAuthService.UserIsAuthed(channel, user));
            await ChannelAuthService.CreateAuth(channel, null);
            await ChannelAuthService.CreateAuth(null, user);
            await ChannelAuthService.CreateAuth(null, null);
            await ChannelAuthService.CreateAuth(new Channel(), new User());
            Assert.IsFalse(await ChannelAuthService.UserIsAuthed(channel, user));
        }

        // User Is Authed

        [Test]
        public async Task ChannelAuthCreatesSuccessfully()
        {
            // Checks an un-authed user returns false
            // Checks an authed user returns true
            // Checks auths can be created

            Assert.IsFalse(await ChannelAuthService.UserIsAuthed(channel, user));
            await ChannelAuthService.CreateAuth(channel, user);
            Assert.IsTrue(await ChannelAuthService.UserIsAuthed(channel, user));
        }

        [Test]
        public async Task CheckAuthWithEmptyParamsIsFalse()
        {
            Assert.IsFalse(await ChannelAuthService.UserIsAuthed(channel, null));
            Assert.IsFalse(await ChannelAuthService.UserIsAuthed(null, user));
        }
    }
}
