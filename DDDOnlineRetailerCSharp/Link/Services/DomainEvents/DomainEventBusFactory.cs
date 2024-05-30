using DDDOnlineRetailerCSharp.Domain;

namespace DDDOnlineRetailerCSharp.Link.Services.DomainEvents;

public static class DomainEventBusFactory
{
    public static DomainDomainEventBus RegisterAll(IDomainEventHandler handler, IUnitOfWork uow)
    {
        DomainDomainEventBus domainDomainEventBus = new(uow);
        // domainDomainEventBus.RegisterHandler<BatchCreated>(handler.HandleAsync);
        // domainDomainEventBus.RegisterHandler<BatchQuantityChanged>(handler.HandleAsync);
        //
        // domainDomainEventBus.RegisterHandler<OutOfStock>(handler.HandleAsync);
        domainDomainEventBus.RegisterHandler<Deallocated>(handler.HandleAsync);
        return domainDomainEventBus;
    }
}