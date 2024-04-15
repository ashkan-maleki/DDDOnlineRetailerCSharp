using DDDOnlineRetailerCSharp.Domain;
using DDDOnlineRetailerCSharp.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DDDOnlineRetailerCSharp.Link.Adaptors;

public class Repository(RetailerDbContext dbContext) : IRepository
{
    public ICollection<Product> Seen { get; } = new List<Product>();

    public async Task AddAsync(Product product)
    {
        await dbContext.Products.AddAsync(product);
        Seen.Add(product);
    }

    public async Task<Product?> GetAsync(string sku)
    {
        Product? product = await dbContext.Products
            .Include(product => product.Batches)
            .ThenInclude(batch => batch.Allocations)
            .FirstOrDefaultAsync(product => product.Sku == sku);
        if (product != null)
        {
            Seen.Add(product);
        }
        return product;
    }
}