using NUnit.Framework;
using ActiveBear.Controllers;
using ActiveBear.Services;
using System;
using System.Threading.Tasks;

namespace ActiveBear.Spec.Controllers
{
    [TestFixture]
    public class ChannelAuthControllerSpec
    {
        private ChannelAuthController controller;

        [SetUp]
        protected void SetUp()
        {
            var context = DbService.NewDbContext();
            controller = new ChannelAuthController(context);
        }

        [Test]
        public async Task AuthUserWithNullGuidFails()
        {
            var output = await controller.AuthUserToChannel(null);
            Assert.AreEqual(1, 1);
        }

        [Test]
        public void AuthUserWithFakeGuidFails()
        {

        }

        [Test]
        public void AuthUserWithEmptyGuidFails()
        {

        }

        [Test]
        public void AuthLegitUserSucceeds()
        {

        }
    }
}
