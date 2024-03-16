using DDDOnlineRetailerCSharp.Domain;
using DDDOnlineRetailerCSharp.Link;

namespace DDDOnlineRetailerCSharp.Presentation.Services;

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

public class BatchService
{
    private bool IsValidSku(string sku, List<Batch> batches) => batches.Any(b => b.Sku == sku);

    public async Task<string> Allocate(OrderLine line, IRepository repo, IUnitOfWork unitOfWork)
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