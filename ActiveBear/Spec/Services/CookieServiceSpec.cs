using System.Threading.Tasks;
using ActiveBear.Models;
using ActiveBear.Services;
using NUnit.Framework;

namespace ActiveBear.Spec.Services
{
    [TestFixture]
    public class CookieServiceSpec
    {
        private ActiveBearContext context;
        private UserService userService;
        internal const string Lorem = "Lorem";

        [SetUp]
        protected void SetUp()
        {
            context = DbService.NewDbContext();
            userService = new UserService(context);
        }

        [TearDown]
        protected void TearDown()
        {
            context.Dispose();
        }

        [Test]
        public async Task CurrentUserReturnsRightUser()
        {
            var user = await userService.CreateUser(Lorem, Lorem, Lorem);

            // TODO: Stub HttpRequest calls
        }
    }
}
