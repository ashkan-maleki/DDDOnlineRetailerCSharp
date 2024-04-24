using DDDOnlineRetailerCSharp.Domain;

namespace DDDOnlineRetailerCSharp.Link.Services;

public interface IEventHandler
{
    public Task HandleAsync(OutOfStock @event);
    public Task HandleAsync(BatchCreated @event);
    public Task HandleAsync(BatchQuantityChanged @event);
    public Task<object> HandleAsync(AllocationRequired @event);
    
}

public class EventHandler(IEmailService emailService, IUnitOfWork uow) : IEventHandler
{
    public  Task HandleAsync(OutOfStock @event)
    {
        emailService.Send($"{@event.Sku} is ran out of stock");
        return Task.FromResult(0);
    }


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

    public async Task<object> HandleAsync(AllocationRequired @event)
    {
        OrderLine line = new(@event.OrderId, @event.Sku, @event.Qty);
        Product? product = await uow.Repository.GetAsync(line.Sku);
        
        if (product == null)
        {
            throw new InvalidSku($"Invalid sku {line.Sku}");
        }
        
        string? batchRef = product.Allocate(line);
        await uow.CommitAsync();
        return batchRef ?? string.Empty;
    }
}