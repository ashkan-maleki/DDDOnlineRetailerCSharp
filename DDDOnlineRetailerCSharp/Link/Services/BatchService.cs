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

public class BatchService(IRepository repo, IUnitOfWork unitOfWork)
{
    private bool IsValidSku(string sku, List<Batch> batches) => batches.Any(b => b.Sku == sku);

    public async Task<string> Allocate(OrderLine line)
    {
        List<Batch> batches = await repo.ListAsync();
        if (!IsValidSku(line.Sku, batches))
        {
            throw new InvalidSku($"Invalid sku {line.Sku}");
        }

        string batchRef = Domain.Domain.Allocate(line, batches);
        await unitOfWork.CommitAsync();
        return batchRef;
    }
}