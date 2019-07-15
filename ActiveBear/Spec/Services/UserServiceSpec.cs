using System;
using System.Threading.Tasks;
using ActiveBear.Hubs;
using ActiveBear.Models;
using ActiveBear.Services;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ActiveBear.Spec.Services
{
    [TestFixture]
    public class UserServiceSpec
    {
        private User user;
        internal const string Lorem = "Lorem";

        [SetUp]
        protected async Task SetUp()
        {
            user = await UserService.CreateUser(Lorem, Lorem, Lorem);
        }

        [Test]
        public void ValidCreateUserSucceeds()
        {
            Assert.IsNotNull(user);
        }

        [Test]
        public async Task InvalidCreateUserFails()
        {
            Assert.IsNull(await UserService.CreateUser(Lorem, "", Lorem));
            Assert.IsNull(await UserService.CreateUser("", Lorem, Lorem));
            Assert.IsNull(await UserService.CreateUser(Lorem, null, Lorem));
        }

        [Test]
        public async Task CreateUserWithExistingNameFails()
        {
            // Create a user to occupy the namespace
            Assert.IsNotNull(user);
            // Try to create user with the same name, expect it to fail
            Assert.IsNull(await UserService.CreateUser(Lorem, Lorem, Lorem));
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
            var foundUser = await UserService.ExistingUser(user.CookieId);
            Assert.IsNotNull(foundUser);

            // Try a get with invalid details, it should fail
            Assert.IsNull(await UserService.ExistingUser(Guid.NewGuid()));
            Assert.IsNull(await UserService.ExistingUser(null));
        }

        [Test]
        public async Task ExistingUserMatchesOnNameOnly()
        {
            var foundUser = await UserService.ExistingUser(Lorem);

            // Get user with correct name & pw
            Assert.AreEqual(user.CookieId, foundUser.CookieId);
            Assert.IsTrue(user.Equals(foundUser));

            // Try to get user with incorrect details
            foundUser = await UserService.ExistingUser("test");
            Assert.IsNull(foundUser);
        }

        [Test]
        public async Task ExistingUserMatchesOnNameAndPw()
        {
            var foundUser = await UserService.ExistingUser(Lorem, Lorem);

            // Get user with correct name & pw
            Assert.AreEqual(user.CookieId, foundUser.CookieId);
            Assert.IsTrue(user.Equals(foundUser));

            // Try to get user with incorrect details
            foundUser = await UserService.ExistingUser(Lorem, "test");
            Assert.IsNull(foundUser);
        }
    }
}
