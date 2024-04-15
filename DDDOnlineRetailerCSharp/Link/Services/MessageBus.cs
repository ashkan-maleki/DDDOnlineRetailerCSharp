using DDDOnlineRetailerCSharp.Domain;

namespace DDDOnlineRetailerCSharp.Link.Services;

// IEventHandler
public interface IMessageBus
{
    void RegisterHandler<T>(Func<T, Task> handler) where T : Event;
    Task HandleAsync(Event @event);
}

// EventHandler
public class MessageBus : IMessageBus
{
    private readonly Dictionary<Type, Func<Event, Task>> _handlers = new();

    public void RegisterHandler<T>(Func<T, Task> handler) where T : Event
    {
        if (_handlers.ContainsKey(typeof(T)))
        {
            throw new IndexOutOfRangeException("You cannot register the same event handler twice");
        }
        _handlers.Add(typeof(T), x => handler((T)x));
    }

    public async Task HandleAsync(Event @event)
    {
        if (_handlers.TryGetValue(@event.GetType(), out Func<Event, Task> handler))
        {
            await handler(@event);
        }

        throw new ArgumentNullException(nameof(handler), "No event handler was registered");
    }
}


public static class MessageBusFactory
{
    public static MessageBus RegisterAll(IEventHandler handler)
    {
        MessageBus messageBus = new();
        messageBus.RegisterHandler<OutOfStock>(handler.HandleAsync);
        return messageBus;
    }
}