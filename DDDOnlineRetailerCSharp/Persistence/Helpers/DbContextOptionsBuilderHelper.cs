using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DDDOnlineRetailerCSharp.Persistence.Helpers;

public static class DbContextOptionsBuilderHelper
{
    public static DbContextOptionsBuilder UseInMemoryDatabaseOptionsBuilder(this DbContextOptionsBuilder builder)
    {
        var dbName = $"RetailerDb_{DateTime.Now.ToFileTimeUtc()}";
        return builder.UseInMemoryDatabase(dbName);
    }
    
    public static DbContextOptionsBuilder UseSqliteDatabaseOptionsBuilder(this DbContextOptionsBuilder builder)
    {
        var cnn = new SqliteConnection("Filename=:memory:");
        cnn.Open();
        return builder.UseSqlite(cnn);
    }
}