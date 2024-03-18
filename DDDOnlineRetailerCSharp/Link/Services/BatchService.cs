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

public class BatchService(IUnitOfWork uow)
{
    private bool IsValidSku(string sku, List<Batch> batches) => batches.Any(b => b.Sku == sku);

    public async Task AddBatch(Batch batch)
    {
        await uow.Repository.AddAsync(batch);
        await uow.CommitAsync();
        List<Batch> batches = await uow.Repository.ListAsync();
        string a = "";
    }

    public async Task<string> Allocate(OrderLine line)
    {
        List<Batch> batches = await uow.Repository.ListAsync();
        if (!IsValidSku(line.Sku, batches))
        {
            throw new InvalidSku($"Invalid sku {line.Sku}");
        }

        string batchRef = Domain.Domain.Allocate(line, batches);
        await uow.CommitAsync();
        return batchRef;
    }
}