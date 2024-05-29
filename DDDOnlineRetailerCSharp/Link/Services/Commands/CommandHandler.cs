using DDDOnlineRetailerCSharp.Domain;
using DDDOnlineRetailerCSharp.Link.Adaptors;

namespace DDDOnlineRetailerCSharp.Link.Services.Commands;

public class CommandHandler(IEmailService emailService, IUnitOfWork uow) : ICommandHandler
{
    public async Task HandleAsync(CreateBatch command)
    {
        Product? product = await uow.Repository.GetAsync(command.Sku);
        if (product == null)
        {
            product = new(command.Sku, new List<Batch>());
            await uow.Repository.AddAsync(product);
        }

        product.Batches.Add(new Batch(command.Reference, command.Sku, command.Qty, command.Eta));
        await uow.CommitAsync();
    }

    public async Task HandleAsync(ChangeBatchQuantity command)
    {
        Product? product = await uow.Repository.GetByBatchRef(command.Reference);
        if (product != null)
        {
            product.ChangeBatchQuantity(command.Reference, command.Qty);
            await uow.CommitAsync();
        }
    }

    public async Task HandleAsync(Allocate command)
    {
        OrderLine line = new(command.OrderId, command.Sku, command.Qty);
        Product? product = await uow.Repository.GetAsync(line.Sku);

        if (product == null)
        {
            throw new InvalidSku($"Invalid sku {line.Sku}");
        }

        string? batchRef = product.Allocate(line);
        await uow.CommitAsync();
    }
}