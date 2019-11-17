using System;
using System.Threading.Tasks;
using ActiveBear.Models;
using Microsoft.EntityFrameworkCore;

namespace ActiveBear.Services
{
    public class UserService : BaseService
    {
        public UserService(ActiveBearContext _context) : base(_context) { }

        public async Task<User> CreateUser(string name, string password, string description)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password)) return null;
            if (await ExistingUser(name) != null) return null;

            var newUser = new User
            {
                Name = name,
                Description = description,
                Password = EncryptionService.Sha256(password)
            };
            if (newUser.CookieId == Guid.Empty) return newUser;

            context.Add(newUser);
            await context.SaveChangesAsync();

            return newUser;
        }

        public async Task<User> ExistingUser(string name, string password)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
                return null;

            var hashedPassword = EncryptionService.Sha256(password);
            return await context.Users.FirstOrDefaultAsync(u => u.Name == name &&
                                                                u.Password == hashedPassword);
        }

        public async Task<User> ExistingUser(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            return await context.Users.FirstOrDefaultAsync(u => u.Name == name);
        }

        public async Task<User> ExistingUser(Guid cookieId)
        {
            if (cookieId == Guid.Empty) return null;
            return await context.Users.FirstOrDefaultAsync(u => u.CookieId == cookieId);
        }
    }
}
