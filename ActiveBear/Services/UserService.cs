using System;
using System.Threading.Tasks;
using ActiveBear.Models;
using Microsoft.EntityFrameworkCore;

namespace ActiveBear.Services
{
    public static class UserService
    {
        public static async Task<User> CreateUser(string name, string password, string description)
        {
            User newUser = null;
            var context = DbService.NewDbContext();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
                return null;

            var existingUser = await ExistingUser(name);
            if (existingUser != null)
                return null;

            newUser = new User
            {
                Name = name,
                Description = description,
                Password = EncryptionService.Sha256(password)
            };
            if (newUser.CookieId == Guid.Empty)
                return newUser;

            context.Add(newUser);
            await context.SaveChangesAsync();

            return newUser;
        }

        public static async Task<User> ExistingUser(string name, string password = "")
        {
            var context = DbService.NewDbContext();
            if (string.IsNullOrEmpty(password))
                return await context.Users.FirstOrDefaultAsync(u => u.Name == name);

            // Match on password too
            var hashedPassword = EncryptionService.Sha256(password);
            return await context.Users.FirstOrDefaultAsync(u => u.Name == name &&
                                                                u.Password == hashedPassword);
        }

        public static async Task<User> ExistingUser(Guid cookieId)
        {
            var context = DbService.NewDbContext();
            if (cookieId == Guid.Empty) return null;

            return await context.Users.FirstOrDefaultAsync(u => u.CookieId == cookieId);
        }
    }
}
