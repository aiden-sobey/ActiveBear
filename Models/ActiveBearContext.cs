using Microsoft.EntityFrameworkCore;

namespace ActiveBear.Models
{
    public class ActiveBearContext : DbContext
    {
        public ActiveBearContext(DbContextOptions<ActiveBearContext> context) : base(context) { }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ChannelAuth> ChannelAuths { get; set; }
    }
}
