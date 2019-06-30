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
                // Look for any messages.
                if (context.Channels.Any())
                    return;   // DB has been seeded

                // Create Users
                var userOne = await UserService.CreateUser("Aiden", "admin", "The admin of the site");
                var userTwo = await UserService.CreateUser("Tom", "NIM", "OG comrade");
                var userThree = await UserService.CreateUser("Claudine", "enc", "The GF");

                // Create channels
                var channel = ChannelService.CreateChannel("TCIP", "activebear", userOne);
                var channelTwo = ChannelService.CreateChannel("SecretRoom", "starcraft", userTwo);
            }
        }
    }
}
