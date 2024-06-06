using DDDOnlineRetailerCSharp.Domain;
using DDDOnlineRetailerCSharp.Link.Adaptors;

namespace DDDOnlineRetailerCSharp.Link.Services.DomainEvents;


public interface IGenericEventHandler<in TEvent> where TEvent : Event
{
    Task HandleAsync(TEvent @event) ;
}


public interface IOutOfStockHandler : IGenericEventHandler<OutOfStock>
{
    new Task HandleAsync(OutOfStock @event);
    Task IGenericEventHandler<OutOfStock>.HandleAsync(OutOfStock @event) => HandleAsync(@event);

}


public class DomainEventHandler(IEmailService emailService, ILogger<DomainEventHandler> logger) : IDomainEventHandler
{
    public Task HandleAsync(OutOfStock @event, IUnitOfWork uow)
    {
        emailService.Send("admin@eshop.com", $"{@event.Sku} is ran out of stock");
        return Task.FromResult(0);
    }

    public async Task HandleAsync(BatchCreated @event, IUnitOfWork uow)
    {
        logger.LogInformation("Publishing integration event: {EventID} - ({@EventType})", @event.Id, typeof(BatchCreated));
        await Task.FromResult(0);
    }

    public async Task HandleAsync(BatchQuantityChanged @event, IUnitOfWork uow)
    {
        logger.LogInformation("Publishing integration event: {EventID} - ({@EventType})", @event.Id, typeof(BatchQuantityChanged));
        await Task.FromResult(0);
    }

    public async Task HandleAsync(Deallocated @event, IUnitOfWork uow)
    {
        OrderLine line = new(@event.OrderId, @event.Sku, @event.Qty);
        Product? product = await uow.Repository.GetAsync(line.Sku);

        if (product == null)
        {
            throw new InvalidSku($"Invalid sku {line.Sku}");
        }

        _ = product.Allocate(line);
        await uow.CommitAsync();
        logger.LogInformation("Publishing integration event: {EventID} - ({@EventType})", @event.Id, typeof(Deallocated));
    }
}