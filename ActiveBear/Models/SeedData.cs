using System;
using System.Collections.Generic;
using System.Linq;
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
                if (context.Message.Any())
                    return;   // DB has been seeded

                // Create Users

                var senderOne = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Aiden",
                    Description = "Admin guy",
                    Password = "12",
                    ChannelAuths = new List<ChannelAuth>()
                };
                var senderTwo = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Tom",
                    Description = "loser power user",
                    Password = "11",
                    ChannelAuths = new List<ChannelAuth>()
                };
                context.Users.AddRange(senderOne, senderTwo);
                context.SaveChanges();

                // Create channel
                var channel = new Channel
                {
                    Id = Guid.NewGuid(),
                    Title = "TCIP",
                    MemberCount = 2,
                    AuthorisedUsers = new List<User> { senderOne, senderTwo },
                    Messages = new List<Message>(),
                    Status = "ACTIVE",
                    KeyHash = "pretend_secure_for_now",
                    CreateDate = DateTime.Now,
                    CreateUser = senderOne
                };
                context.Channels.Add(channel);

                // Create messages

                context.Message.AddRange(
                    new Message
                    {
                        Id = Guid.NewGuid(),
                        Sender = senderOne.Id,
                        Channel = channel.Id,
                        EncryptedContents = "BigYeet",
                        SendDate = DateTime.Now
                    },

                    new Message
                    {
                        Id = Guid.NewGuid(),
                        Sender = senderOne.Id,
                        Channel = channel.Id,
                        EncryptedContents = "Example message",
                        SendDate = DateTime.Now
                    },

                    new Message
                    {
                        Id = Guid.NewGuid(),
                        Sender = senderTwo.Id,
                        Channel = channel.Id,
                        EncryptedContents = "Active bear yo",
                        SendDate = DateTime.Now
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
