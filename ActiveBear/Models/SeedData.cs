using System;
using System.Collections.Generic;
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
                serviceProvider.GetRequiredService<
                    DbContextOptions<ActiveBearContext>>()))
            {
                // Look for any messages.
                if (context.Messages.Any())
                    return;   // DB has been seeded

                // Create Users
                var userOne = UserService.CreateUser("Aiden", "admin", "The admin of the site", context);
                var userTwo = UserService.CreateUser("Tom", "NIM", "OG comrade", context);
                var userThree = UserService.CreateUser("Claudine", "enc", "The GF", context);

                // Create channels
                var channel = ChannelService.CreateChannel("TCIP", "activebear", context);

                // Create messages

                var messageOne = MessageService.NewMessage(userOne, channel, "small yeet", context);
                var messageTwo = MessageService.NewMessage(userTwo, channel, "yeet", context);
                var messageTre = MessageService.NewMessage(userOne, channel, "bigger yeet", context);
                var messageQua = MessageService.NewMessage(userThree, channel, "standard message", context);
            }
        }
    }
}
