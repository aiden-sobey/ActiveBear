using System;
using System.Linq;
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

            var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Name == name);
            if (existingUser != null)
                return null;

            newUser = new User
            {
                Name = name,
                Description = description,
                Password = password
            };
            if (newUser.CookieId == Guid.Empty)
                return newUser;

            context.Add(newUser);
            await context.SaveChangesAsync();

            return newUser;
        }
    }
}
