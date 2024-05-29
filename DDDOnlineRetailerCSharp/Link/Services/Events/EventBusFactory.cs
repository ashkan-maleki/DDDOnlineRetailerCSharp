using DDDOnlineRetailerCSharp.Domain;

namespace DDDOnlineRetailerCSharp.Link.Services.Events;

public static class EventBusFactory
{
    public static EventBus RegisterAll(IEventHandler handler, IUnitOfWork uow)
    {
        EventBus eventBus = new(uow);
        eventBus.RegisterHandler<BatchCreated>(handler.HandleAsync);
        eventBus.RegisterHandler<BatchQuantityChanged>(handler.HandleAsync);

        eventBus.RegisterHandler<OutOfStock>(handler.HandleAsync);
        eventBus.RegisterHandler<AllocationRequired>(handler.HandleAsync);
        return eventBus;
    }
}