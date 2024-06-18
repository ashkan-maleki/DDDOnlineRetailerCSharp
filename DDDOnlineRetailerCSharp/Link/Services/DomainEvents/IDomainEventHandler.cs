using DDDOnlineRetailerCSharp.Domain;

namespace DDDOnlineRetailerCSharp.Link.Services.DomainEvents;

public interface IDomainEventHandler
{
    public Task HandleAsync(OutOfStockDomainEvent @event, IUnitOfWork uow);
    public Task HandleAsync(BatchCreatedDomainEvent @event, IUnitOfWork uow);
    public Task HandleAsync(BatchQuantityChangedDomainEvent @event, IUnitOfWork uow);
    public Task HandleAsync(DeallocatedDomainEvent @event, IUnitOfWork uow);
    public Task HandleAsync(AllocatedDomainEvent @event, IUnitOfWork uow);
}