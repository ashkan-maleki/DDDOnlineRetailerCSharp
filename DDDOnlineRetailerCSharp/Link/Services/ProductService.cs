using DDDOnlineRetailerCSharp.Domain;
using DDDOnlineRetailerCSharp.Link.Adaptors;

namespace DDDOnlineRetailerCSharp.Link.Services;

public class InvalidSku : Exception
{
    public InvalidSku()
    {
    }

    public InvalidSku(string message)
        : base(message)
    {
    }

    public InvalidSku(string message, Exception inner)
        : base(message, inner)
    {
    }
}

public class ProductService(IUnitOfWork uow)
{
    private bool IsValidSku(string sku, List<Batch> batches) => batches.Any(b => b.Sku == sku);

    public async Task AddBatch(Batch batch)
    {
        Product? product = await uow.Repository.GetAsync(batch.Sku);
        if (product == null)
        {
            product = new(batch.Sku, new List<Batch>());
            await uow.Repository.AddAsync(product);
        } 
        product.Batches.Add(batch);
        await uow.CommitAsync();
        // List<Batch> batches = await uow.Repository.ListAsync();
        
    }

    public async Task<string> Allocate(OrderLine line)
    {
        Product? product = await uow.Repository.GetAsync(line.Sku);
        
        if (product == null)
        {
            throw new InvalidSku($"Invalid sku {line.Sku}");
        }
        
        string batchRef = product.Allocate(line);
        await uow.CommitAsync();
        return batchRef;
    }
}