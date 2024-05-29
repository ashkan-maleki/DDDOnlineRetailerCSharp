using DDDOnlineRetailerCSharp.Domain;

namespace DDDOnlineRetailerCSharp.Link.Services.Events;

public interface IEventHandler
{
    public Task HandleAsync(OutOfStock @event);
    public Task HandleAsync(BatchCreated @event);
    public Task HandleAsync(BatchQuantityChanged @event);
    public Task HandleAsync(AllocationRequired @event);
}