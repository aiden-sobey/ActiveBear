using System.Threading.Tasks;
using NUnit.Framework;

namespace ActiveBear.Spec.Services
{
    [TestFixture]
    public class CookieServiceSpec : ServiceSpec
    {
        [Test]
        public async Task CurrentUserReturnsRightUser()
        {
            var user = await userService.CreateUser(Lorem, Lorem, Lorem);

            // TODO: Stub HttpRequest calls
        }
    }
}
