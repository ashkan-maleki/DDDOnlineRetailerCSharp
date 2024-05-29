using DDDOnlineRetailerCSharp.Domain;
using DDDOnlineRetailerCSharp.Link.Adaptors;

namespace DDDOnlineRetailerCSharp.Link.Services.Events;

public class EventHandler(IEmailService emailService, IUnitOfWork uow) : IEventHandler
{
    public Task HandleAsync(OutOfStock @event) => Task.FromResult((object)emailService.Send($"{@event.Sku} is ran out of stock"));


    public async Task HandleAsync(BatchCreated @event)
    {
        Product? product = await uow.Repository.GetAsync(@event.Sku);
        if (product == null)
        {
            product = new(@event.Sku, new List<Batch>());
            await uow.Repository.AddAsync(product);
        }

        product.Batches.Add(new Batch(@event.Reference, @event.Sku, @event.Qty, @event.Eta));
        await uow.CommitAsync();
    }

    public async Task HandleAsync(BatchQuantityChanged @event)
    {
        Product? product = await uow.Repository.GetByBatchRef(@event.Reference);
        if (product != null)
        {
            product.ChangeBatchQuantity(@event.Reference, @event.Qty);
            await uow.CommitAsync();
        }
    }

    public async Task HandleAsync(AllocationRequired @event)
    {
        OrderLine line = new(@event.OrderId, @event.Sku, @event.Qty);
        Product? product = await uow.Repository.GetAsync(line.Sku);

        if (product == null)
        {
            throw new InvalidSku($"Invalid sku {line.Sku}");
        }

        string? batchRef = product.Allocate(line);
        await uow.CommitAsync();
    }
}