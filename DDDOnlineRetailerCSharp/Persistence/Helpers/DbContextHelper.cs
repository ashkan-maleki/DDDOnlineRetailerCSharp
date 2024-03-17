using Microsoft.EntityFrameworkCore;

namespace DDDOnlineRetailerCSharp.Persistence.Helpers;

public static class DbContextHelper
{
    public static DbContextOptionsBuilder<T> UseInMemoryDatabaseOptionsBuilder<T>(this DbContext builder) where T : DbContext
    {
        var dbName = $"RetailerDb_{DateTime.Now.ToFileTimeUtc()}";
        return new DbContextOptionsBuilder<T>().UseInMemoryDatabase(dbName);
    }
}