using System;
using System.Linq;
using System.Threading.Tasks;
using ActiveBear.Hubs;
using ActiveBear.Models;
using ActiveBear.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
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
