using System;
using ActiveBear.Models;

namespace ActiveBear.Services
{
    public static class UserService
    {
        public static User CreateUser(string name, string password, string description, ActiveBearContext context)
        {
            if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(password))
                return null;

            var newUser = new User
            {
                Name = name,
                Description = description,
                Password = password //TODO: hash this
            };

            context.Add(newUser);
            context.SaveChanges();

            return newUser; //TODO: return null if user creation fails
        }
    }
}
