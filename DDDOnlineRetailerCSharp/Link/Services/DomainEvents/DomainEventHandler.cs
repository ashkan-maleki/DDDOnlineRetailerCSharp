using DDDOnlineRetailerCSharp.Application;
using DDDOnlineRetailerCSharp.Domain;
using DDDOnlineRetailerCSharp.EventBus;
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


public class OutOfStockHandler : IOutOfStockHandler
{
    public Task HandleAsync(OutOfStockDomainEvent @event)
    {
        throw new NotImplementedException();
    }
}

public class NewDomainEventBus
{
    private readonly Dictionary<Type, Func<DomainEvent, IUnitOfWork, Task>> _handlers = new();

    public void RegisterHandler<T>(Func<T, IUnitOfWork, Task> handler) where T : DomainEvent
    {
        if (_handlers.ContainsKey(typeof(T)))
        {
            throw new IndexOutOfRangeException("You cannot register the same event handler twice");
        }

        _handlers.Add(typeof(T), (x, uow) => handler((T)x, uow));
    }

    public async Task HandleAsync(DomainEvent @event, IUnitOfWork uow)
    {
        if (_handlers.TryGetValue(@event.GetType(), out Func<DomainEvent, IUnitOfWork, Task>? handler))
        {
            await handler(@event, uow);
        }
        else
        {
            throw new ArgumentNullException(nameof(handler), "No event handler was registered");
        }
    }
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
        DeallocatedIntegrationEvent deallocatedIntegrationEvent = new(@event.OrderId, @event.Sku, @event.Reference);
        await eventBus.HandleAsync(deallocatedIntegrationEvent);
        logger.LogInformation("Published domain event: {EventID} - ({@EventType})", @event.Id, typeof(DeallocatedDomainEvent));
    }

    public async Task HandleAsync(AllocatedDomainEvent @event, IUnitOfWork uow)
    {
        AllocatedIntegrationEvent allocatedIntegrationEvent = new(@event.OrderId, @event.Sku, @event.Reference);
        await eventBus.HandleAsync(allocatedIntegrationEvent);
        logger.LogInformation("Published domain event: {EventID} - ({@EventType})", @event.Id, typeof(AllocatedDomainEvent));
    }
}