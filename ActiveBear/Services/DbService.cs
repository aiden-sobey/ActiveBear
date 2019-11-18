using ActiveBear.Models;
using Microsoft.EntityFrameworkCore;

namespace ActiveBear.Services
{
    public class DbService
    {
        public static ActiveBearContext NewDbContext()
        {
            var connectionString = "Data Source=ActiveBear.db"; // TODO: find how to reference the constant here
            var options = new DbContextOptionsBuilder<ActiveBearContext>();
            options.UseSqlite(connectionString);
            return new ActiveBearContext(options.Options);
        }

        public static ActiveBearContext NewTestContext()
        {
            var options = new DbContextOptionsBuilder<ActiveBearContext>()
                .UseInMemoryDatabase(databaseName: "write_to_db")
                .Options;

            return new ActiveBearContext(options);
        }
    }
}
