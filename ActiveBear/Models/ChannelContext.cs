using System;
using Microsoft.EntityFrameworkCore;
namespace ActiveBear.Models
{
    public class ChannelContext : DbContext
    {
        public ChannelContext(DbContextOptions<ChannelContext> context) : base(context) { }

        public ChannelContext()
        {
        }

        public DbSet<Channel> Channel { get; set; }
    }
}