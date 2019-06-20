using ActiveBear.Models;
using Microsoft.EntityFrameworkCore;

namespace ActiveBear.Services
{
    public class DbService
    {
        public static ActiveBearContext NewDbContext()
        {
            var connectionString = "Data Source=ActiveBear.db";
            var options = new DbContextOptionsBuilder<ActiveBearContext>();
            options.UseSqlite(connectionString);
            return new ActiveBearContext(options.Options);
        }
    }
}
