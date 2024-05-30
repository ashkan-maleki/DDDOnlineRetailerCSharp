using DDDOnlineRetailerCSharp.Domain;
using DDDOnlineRetailerCSharp.Link.Services.DomainEvents;

namespace DDDOnlineRetailerCSharp.Link.Services.IntegrationEvents;

public static class IntegrationEventBusFactory
{
    public static DomainDomainEventBus RegisterAll(IDomainEventHandler handler, IUnitOfWork uow)
    {
        DomainDomainEventBus domainDomainEventBus = new(uow);
        // domainDomainEventBus.RegisterHandler<BatchCreated>(handler.HandleAsync);
        // domainDomainEventBus.RegisterHandler<BatchQuantityChanged>(handler.HandleAsync);
        //
        // domainDomainEventBus.RegisterHandler<OutOfStock>(handler.HandleAsync);
        // domainDomainEventBus.RegisterHandler<Deallocated>(handler.HandleAsync);
        return domainDomainEventBus;
    }
}