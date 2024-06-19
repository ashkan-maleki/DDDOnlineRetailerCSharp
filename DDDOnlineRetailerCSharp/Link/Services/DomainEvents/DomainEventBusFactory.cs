using DDDOnlineRetailerCSharp.Domain;

namespace DDDOnlineRetailerCSharp.Link.Services.DomainEvents;

public static class DomainEventBusFactory
{
    public static DomainEventBus RegisterAll(IDomainEventHandler handler)
    {
        DomainEventBus domainEventBus = new();
        // domainDomainEventBus.RegisterHandler<BatchCreated>(handler.HandleAsync);
        // domainDomainEventBus.RegisterHandler<BatchQuantityChanged>(handler.HandleAsync);
        //
        domainEventBus.RegisterHandler<OutOfStockDomainEvent>(handler.HandleAsync);
        domainEventBus.RegisterHandler<BatchCreatedDomainEvent>(handler.HandleAsync);
        domainEventBus.RegisterHandler<BatchQuantityChangedDomainEvent>(handler.HandleAsync);
        domainEventBus.RegisterHandler<AllocatedDomainEvent>(handler.HandleAsync);
        domainEventBus.RegisterHandler<DeallocatedDomainEvent>(handler.HandleAsync);
        return domainEventBus;
    }
}