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

        private ActiveBearContext context;
        private UserService userService;
        private ChannelService channelService;
        private ChannelAuthService authService;

        private const string Lorem = "Lorem";

        [SetUp]
        protected async Task SetUp()
        {
            context = DbService.NewTestContext();
            userService = new UserService(context);
            channelService = new ChannelService(context);
            authService = new ChannelAuthService(context);

            user = await userService.CreateUser(Lorem, Lorem, Lorem);
            channel = await channelService.CreateChannel(Lorem, Lorem, user);
        }

        [TearDown]
        protected void TearDown()
        {
            context.Dispose();
        }

        // Create Auth

        [Test]
        public async Task CreateAuthForAlreadyAuthedUserExits()
        {
            // Create auth
            await authService.CreateAuth(channel, user);

            // Try create a duplicate auth
            var authResult = await authService.CreateAuth(channel, user);
            Assert.IsFalse(authResult);
        }

        [Test]
        public async Task CreateAuthWithEmptyParamsExits()
        {
            Assert.IsFalse(await authService.UserIsAuthed(channel, user));
            await authService.CreateAuth(channel, null);
            await authService.CreateAuth(null, user);
            await authService.CreateAuth(null, null);
            await authService.CreateAuth(new Channel(), new User());
            Assert.IsFalse(await authService.UserIsAuthed(channel, user));
        }

        // User Is Authed

        [Test]
        public async Task ChannelAuthCreatesSuccessfully()
        {
            // Checks an un-authed user returns false
            // Checks an authed user returns true
            // Checks auths can be created

            Assert.IsFalse(await authService.UserIsAuthed(channel, user));
            await authService.CreateAuth(channel, user);
            Assert.IsTrue(await authService.UserIsAuthed(channel, user));
        }

        [Test]
        public async Task CheckAuthWithEmptyParamsIsFalse()
        {
            Assert.IsFalse(await authService.UserIsAuthed(channel, null));
            Assert.IsFalse(await authService.UserIsAuthed(null, user));
        }
    }
}
