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
        logger.LogInformation("Published domain event: {EventID} - ({@EventType})", @event.Id, typeof(OutOfStockDomainEvent));
        return Task.FromResult(0);
    }

    public async Task HandleAsync(BatchCreatedDomainEvent @event, IUnitOfWork uow)
    {
        logger.LogInformation("Published domain event: {EventID} - ({@EventType})", @event.Id, typeof(BatchCreatedDomainEvent));
        await Task.FromResult(0);
    }

    public async Task HandleAsync(BatchQuantityChangedDomainEvent @event, IUnitOfWork uow)
    {
        logger.LogInformation("Published domain event: {EventID} - ({@EventType})", @event.Id, typeof(BatchQuantityChangedDomainEvent));
        await Task.FromResult(0);
    }

    public async Task HandleAsync(DeallocatedDomainEvent @event, IUnitOfWork uow)
    {
        OrderLine line = new(@event.OrderId, @event.Sku, @event.Qty);
        Product? product = await uow.Repository.GetAsync(line.Sku);

        if (product == null)
        {
            throw new InvalidSku($"Invalid sku {line.Sku}");
        }

        _ = product.Allocate(line);
        await uow.CommitAsync();
        DeallocatedIntegrationEvent deallocatedIntegrationEvent = new(@event.OrderId, @event.Sku, @event.Qty);
        await eventBus.HandleAsync(deallocatedIntegrationEvent);
        logger.LogInformation("Published domain event: {EventID} - ({@EventType})", @event.Id, typeof(DeallocatedDomainEvent));
    }

    public async Task HandleAsync(AllocatedDomainEvent @event, IUnitOfWork uow)
    {
        AllocatedIntegrationEvent allocatedIntegrationEvent = new(@event.OrderId, @event.Sku, @event.Qty, @event.Reference);
        await eventBus.HandleAsync(allocatedIntegrationEvent);
       
        logger.LogInformation("Published domain event: {EventID} - ({@EventType})", @event.Id, typeof(AllocatedDomainEvent));
    }
}