using System;
using System.Linq;
using ActiveBear.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ActiveBear.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ActiveBearContext(
                serviceProvider.GetRequiredService<DbContextOptions<ActiveBearContext>>()))
            {
                // Look for any messages.
                if (context.Channels.Any())
                    return;   // DB has been seeded

                // Create Users
                var userOne = UserService.CreateUser("Aiden", "admin", "The admin of the site");
                var userTwo = UserService.CreateUser("Tom", "NIM", "OG comrade");
                var userThree = UserService.CreateUser("Claudine", "enc", "The GF");

                // Create channels
                var channel = ChannelService.CreateChannel("TCIP", "activebear", userOne);
                var channelTwo = ChannelService.CreateChannel("SecretRoom", "starcraft", userTwo);
            }
        }
    }
}
