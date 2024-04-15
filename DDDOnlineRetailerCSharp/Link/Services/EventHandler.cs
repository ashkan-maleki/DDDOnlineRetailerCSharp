using DDDOnlineRetailerCSharp.Domain;

namespace DDDOnlineRetailerCSharp.Link.Services;

public interface IEventHandler
{
    public Task HandleAsync(OutOfStock @event);
}

public class EventHandler : IEventHandler
{
    public Task HandleAsync(OutOfStock @event)
    {
        throw new NotImplementedException();
    }
}