using DDDOnlineRetailerCSharp.Domain;
using DDDOnlineRetailerCSharp.Persistence.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DDDOnlineRetailerCSharp.Persistence;

public class RetailerDbContext : DbContext
{

    public RetailerDbContext(DbContextOptions<RetailerDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    public static DbContextOptions<RetailerDbContext> CreateInMemoryDbContextOptions()
    {
        var dbName = $"RetailerDb_{DateTime.Now.ToFileTimeUtc()}";
        DbContextOptions<RetailerDbContext> dbContextOptions = new DbContextOptionsBuilder<RetailerDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        return dbContextOptions;
    }
    
    public static DbContextOptions<RetailerDbContext> CreateSqliteDbContextOptions()
    {
        var cnn = new SqliteConnection("Filename=:memory:");
        cnn.Open();
        DbContextOptions<RetailerDbContext> dbContextOptions = new DbContextOptionsBuilder<RetailerDbContext>()
            .UseSqlite(cnn)
            .Options;
        return dbContextOptions;
    }

    public static RetailerDbContext CreateRetailerDbContext(DbContextOptions<RetailerDbContext> options) => new(options);
    public static RetailerDbContext CreateInMemoryRetailerDbContext() => new(CreateInMemoryDbContextOptions());
    public static RetailerDbContext CreateSqliteRetailerDbContext() => new(CreateSqliteDbContextOptions());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BatchEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderLineEntityTypeConfiguration());
    }

    public DbSet<Batch> Batches { get; set; }
    public DbSet<OrderLine> OrderLines { get; set; }

}