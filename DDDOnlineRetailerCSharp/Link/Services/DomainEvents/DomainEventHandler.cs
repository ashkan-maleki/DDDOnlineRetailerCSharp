using DDDOnlineRetailerCSharp.Application;
using DDDOnlineRetailerCSharp.Domain;
using DDDOnlineRetailerCSharp.EventBus;
using DDDOnlineRetailerCSharp.Link.Adaptors;
using DDDOnlineRetailerCSharp.Link.Services.IntegrationEvents;

namespace DDDOnlineRetailerCSharp.Link.Services.DomainEvents;


public interface IGenericEventHandler<in TEvent> where TEvent : Event
{
    Task HandleAsync(TEvent @event) ;
}


public interface IOutOfStockHandler : IGenericEventHandler<OutOfStockDomainEvent>
{
    new Task HandleAsync(OutOfStockDomainEvent @event);
    Task IGenericEventHandler<OutOfStockDomainEvent>.HandleAsync(OutOfStockDomainEvent @event) => HandleAsync(@event);

}


public class DomainEventHandler(IIntegrationEventBus eventBus, ILogger<DomainEventHandler> logger) : IDomainEventHandler
{
    public Task HandleAsync(OutOfStockDomainEvent @event, IUnitOfWork uow)
    {
        OutOfStockIntegrationEvent integrationEvent = new(@event.Sku);
        eventBus.HandleAsync(integrationEvent);
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