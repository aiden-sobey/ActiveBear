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
    public class DbServiceSpec
    {
        internal const string Lorem = "Lorem";

        [Test]
        public async Task NewDbContextCreatedSuccessfully()
        {
            var user = await UserService.CreateUser(Lorem, Lorem, Lorem);
            await ChannelService.CreateChannel(Lorem, Lorem, user);

            var context = DbService.NewDbContext();
            Assert.IsNotEmpty(context.Users.ToList());
            Assert.IsNotEmpty(context.Channels.ToList());
        }
    }
}
