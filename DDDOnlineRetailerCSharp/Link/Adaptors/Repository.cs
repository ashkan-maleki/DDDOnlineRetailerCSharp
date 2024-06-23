using DDDOnlineRetailerCSharp.Domain;
using DDDOnlineRetailerCSharp.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DDDOnlineRetailerCSharp.Link.Adaptors;

public class Repository(RetailerDbContext dbContext) : IRepository
{
    public Dictionary<string, Product> Seen { get; } = new();

    public async Task AddAsync(Product product)
    {
        await dbContext.Products.AddAsync(product);
        AddProductAsSeen(product);
        // Seen.Add(product);
    }

    public async Task<Product?> GetAsync(string sku)
    {
        Product? product = await dbContext.Products
            .Include(product => product.Batches)
            .ThenInclude(batch => batch.Allocations)
            .FirstOrDefaultAsync(product => product.Sku == sku);
        if (product != null)
        {
            AddProductAsSeen(product);
        }
        return product;
    }

    public async Task<Product?> GetByBatchRef(string reference)
    {
        Product? product = await dbContext.Products
            .Include(product => product.Batches)
            .ThenInclude(batch => batch.Allocations)
            .Where(prodcut => prodcut.Batches.Any(batch => batch.Reference == reference))
            .FirstOrDefaultAsync();
        if (product != null)
        {
            AddProductAsSeen(product);
        }

        return product;
    }

    private void AddProductAsSeen(Product product) => Seen[product.Sku] = product;
}