using System;
using System.Linq;
using ActiveBear.Models;

namespace ActiveBear.Services
{
    public static class UserService
    {
        public static User CreateUser(string name, string password, string description)
        {
            User newUser = null;
            var context = DbService.NewDbContext();

            if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(password))
                return null;

            if (context.Users.Where(u => u.Name == name).Any())
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
            context.SaveChanges();

            return newUser;
        }
    }
}
