using DDDOnlineRetailerCSharp.Domain;
using DDDOnlineRetailerCSharp.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DDDOnlineRetailerCSharp.Link.Adaptors;

public class Repository(RetailerDbContext dbContext) : IRepository
{
    public async Task AddAsync(Product product) => await dbContext.Products.AddAsync(product);

    public async Task<Product?> GetAsync(string sku) => await dbContext.Products
        .Include(product => product.Batches)
        .ThenInclude(batch => batch.Allocations)
        .FirstAsync(product => product.Sku == sku);
}