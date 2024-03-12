using DDDOnlineRetailerCSharp.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace DDDOnlineRetailerCSharp.Persistence;

public class RetailerContext(IConfiguration configuration) : DbContext
{
    private readonly IConfiguration _configuration = configuration;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseInMemoryDatabase("RetailerDb");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BatchEntityTypeConfiguration());
    }
    
    
}