using System;
using System.Linq;
using System.Threading.Tasks;
using ActiveBear.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ActiveBear.Models
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ActiveBearContext(
                serviceProvider.GetRequiredService<DbContextOptions<ActiveBearContext>>()))
            {
                var userService = new UserService(context);
                var channelService = new ChannelService(context);

                // Look for any messages.
                if (context.Channels.Any())
                    return;   // DB has been seeded
                
                // Create Users
                var userOne = await userService.CreateUser("UserOne", "abcdefg", "The first user.");
                var userTwo = await userService.CreateUser("UserTwo", "qwerty", "The second user.");

                // Create channels
                var channel = await channelService.CreateChannel("Public", "general", userOne);
                var channelTwo = await channelService.CreateChannel("SecretRoom", "starcraft", userTwo);
            }
        }
    }
}
