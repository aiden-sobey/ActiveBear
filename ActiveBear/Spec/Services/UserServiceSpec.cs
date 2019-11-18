using System;
using System.Threading.Tasks;
using ActiveBear.Services;
using NUnit.Framework;

namespace ActiveBear.Spec.Services
{
    [TestFixture]
    public class UserServiceSpec : ServiceSpec
    {
        [Test]
        public void ValidCreateUserSucceeds()
        {
            Assert.IsNotNull(user);
        }

        [Test]
        public async Task InvalidCreateUserFails()
        {
            Assert.IsNull(await userService.CreateUser(Lorem, "", Lorem));
            Assert.IsNull(await userService.CreateUser("", Lorem, Lorem));
            Assert.IsNull(await userService.CreateUser(Lorem, null, Lorem));
        }

        [Test]
        public async Task CreateUserWithExistingNameFails()
        {
            // Create a user to occupy the namespace
            Assert.IsNotNull(user);
            // Try to create user with the same name, expect it to fail
            Assert.IsNull(await userService.CreateUser(Lorem, Lorem, Lorem));
        }

        [Test]
        public void CreateUserHashesPassword()
        {
            var hashedPassword = EncryptionService.Sha256(Lorem);
            Assert.AreEqual(hashedPassword, user.Password);
        }

        [Test]
        public void CreateUserPopulatesCookieId()
        {
            Assert.IsNotNull(user.CookieId);
            Assert.AreNotEqual(Guid.Empty, user.CookieId);
        }

        [Test]
        public async Task ExistingUserFromCookieWorks()
        {
            // Get the existing user
            var foundUser = await userService.ExistingUser(user.CookieId);
            Assert.IsNotNull(foundUser);

            // Try a get with invalid details, it should fail
            Assert.IsNull(await userService.ExistingUser(Guid.NewGuid()));
            Assert.IsNull(await userService.ExistingUser(null));
        }

        [Test]
        public async Task ExistingUserMatchesOnNameOnly()
        {
            var foundUser = await userService.ExistingUser(Lorem);

            // Get user with correct name & pw
            Assert.AreEqual(user.CookieId, foundUser.CookieId);
            Assert.IsTrue(user.Equals(foundUser));

            // Try to get user with incorrect details
            foundUser = await userService.ExistingUser("test");
            Assert.IsNull(foundUser);
        }

        [Test]
        public async Task ExistingUserMatchesOnNameAndPw()
        {
            var foundUser = await userService.ExistingUser(Lorem, Lorem);

            // Get user with correct name & pw
            Assert.AreEqual(user.CookieId, foundUser.CookieId);
            Assert.IsTrue(user.Equals(foundUser));

            // Try to get user with incorrect details
            foundUser = await userService.ExistingUser(Lorem, "test");
            Assert.IsNull(foundUser);
        }
    }
}
