using DDDOnlineRetailerCSharp.Domain;

namespace DDDOnlineRetailerCSharp.Link.Services.DomainEvents;

public interface IDomainEventHandler
{
    public Task HandleAsync(OutOfStock @event, IUnitOfWork uow);
    public Task HandleAsync(BatchCreated @event, IUnitOfWork uow);
    public Task HandleAsync(BatchQuantityChanged @event, IUnitOfWork uow);
    public Task HandleAsync(Deallocated @event, IUnitOfWork uow);
}