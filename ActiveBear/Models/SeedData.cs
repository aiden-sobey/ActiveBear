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
                if (context.Messages.Any())
                    return;   // DB has been seeded

                // Create Users
                var userOne = UserService.CreateUser("Aiden", "admin", "The admin of the site", context);
                var userTwo = UserService.CreateUser("Tom", "NIM", "OG comrade", context);
                var userThree = UserService.CreateUser("Claudine", "enc", "The GF", context);

                // Create channels
                var channel = ChannelService.CreateChannel("TCIP", "activebear", context, userOne);
                var channelTwo = ChannelService.CreateChannel("SecretRoom", "starcraft", context, userTwo);

                // Create messages
                var messageOne = MessageService.NewMessage(userOne.Name, channel.Id, "small yeet");
                var messageTwo = MessageService.NewMessage(userTwo.Name, channel.Id, "yeet");
                var messageTre = MessageService.NewMessage(userOne.Name, channel.Id, "bigger yeet");
                var messageQua = MessageService.NewMessage(userThree.Name, channel.Id, "standard message");
            }
        }
    }
}
