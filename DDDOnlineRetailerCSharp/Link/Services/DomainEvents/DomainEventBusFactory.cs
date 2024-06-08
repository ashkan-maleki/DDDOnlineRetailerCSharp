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
        domainEventBus.RegisterHandler<Deallocated>(handler.HandleAsync);
        return domainEventBus;
    }
}