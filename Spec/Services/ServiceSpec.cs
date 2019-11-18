using System.Threading.Tasks;
using ActiveBear.Hubs;
using ActiveBear.Models;
using ActiveBear.Services;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ActiveBear.Spec.Services
{
    [TestFixture]
    public class ServiceSpec
    {
        internal User user;
        internal Channel channel;

        internal ActiveBearContext context;
        internal UserService userService;
        internal ChannelService channelService;
        internal MessageService messageService;
        internal ChannelAuthService authService;

        internal const string Lorem = "Lorem";
        internal string packet;

        [SetUp]
        protected async Task SetUp()
        {
            context = DbService.NewTestContext();
            userService = new UserService(context);
            channelService = new ChannelService(context);
            messageService = new MessageService(context);
            authService = new ChannelAuthService(context);

            user = await userService.ExistingUser(Lorem);
            if (user == null)
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
    }
}
