using System.Threading.Tasks;
using ActiveBear.Services;
using NUnit.Framework;

namespace ActiveBear.Spec.Services
{
    [TestFixture]
    public class CookieServiceSpec
    {
        internal const string Lorem = "Lorem";

        [Test]
        public async Task CurrentUserReturnsRightUser()
        {
            var user = await UserService.CreateUser(Lorem, Lorem, Lorem);

            // TODO: Stub HttpRequest calls
        }
    }
}
