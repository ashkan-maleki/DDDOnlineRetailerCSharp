using DDDOnlineRetailerCSharp.Domain;

namespace DDDOnlineRetailerCSharp.Link.Services;

// IEventHandler
public interface IMessageBus
{
    void RegisterHandler<T>(Func<T, Task> handler) where T : Event;
    void RegisterHandler<T>(Func<T, Task<object>> handler) where T : Event;
    Task<Queue<object>> HandleAsync(Event @event);
}

// EventHandler
public class MessageBus(IUnitOfWork uow) : IMessageBus
{
    private readonly Dictionary<Type, Func<Event, Task>> _handlers = new();
    private readonly Dictionary<Type, Func<Event, Task<object>>> _genericHandlers = new();

    public void RegisterHandler<T>(Func<T, Task> handler) where T : Event
    {
        if (_handlers.ContainsKey(typeof(T)))
        {
            throw new IndexOutOfRangeException("You cannot register the same event handler twice");
        }

        _handlers.Add(typeof(T), x => handler((T)x));
    }

    public void RegisterHandler<T>(Func<T, Task<object>> handler) where T : Event  
    {
        if (_genericHandlers.ContainsKey(typeof(T)))
        {
            throw new IndexOutOfRangeException("You cannot register the same event handler twice");
        }

        _genericHandlers.Add(typeof(T), x => handler((T)x));
        
    }

    // public async Task HandleAsync(Event @event)
    // {
    //     Queue<Event> events = new Queue<Event>(new[] { @event });
    //     while (events.Any())
    //     {
    //         @event = events.Dequeue();
    //         if (_handlers.TryGetValue(@event.GetType(), out Func<Event, Task>? handler))
    //         {
    //             await handler(@event);
    //             await foreach (var collectedEvent in  uow.CollectNewEvents())
    //             {
    //                 events.Enqueue(collectedEvent.Result);
    //             }
    //         }
    //         else
    //         {
    //             throw new ArgumentNullException(nameof(handler), "No event handler was registered");
    //         }
    //     } ;
    // }
    
    public async Task<Queue<object>> HandleAsync(Event @event)
    {
        Queue<Event> events = new Queue<Event>(new[] { @event });
        Queue<object> results = new();
        while (events.Any())
        {
            @event = events.Dequeue();
            if (_genericHandlers.TryGetValue(@event.GetType(), out Func<Event, Task<object>>? genHandler))
            {
                results.Enqueue(await genHandler(@event));
                
                await foreach (var collectedEvent in  uow.CollectNewEvents())
                {
                    events.Enqueue(collectedEvent.Result);
                }
            } else if (_handlers.TryGetValue(@event.GetType(), out Func<Event, Task>? handler))
            {
                await handler(@event);
                
                await foreach (var collectedEvent in  uow.CollectNewEvents())
                {
                    events.Enqueue(collectedEvent.Result);
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(handler), "No event handler was registered");
            }
        }

        return results;
    }
}

public static class MessageBusFactory
{
    public static MessageBus RegisterAll(IEventHandler handler, IUnitOfWork uow)
    {
        MessageBus messageBus = new(uow);
        messageBus.RegisterHandler<BatchCreated>(handler.HandleAsync);
        messageBus.RegisterHandler<BatchQuantityChanged>(handler.HandleAsync);
        
        messageBus.RegisterHandler<OutOfStock>(handler.HandleAsync);
        messageBus.RegisterHandler<AllocationRequired>(handler.HandleAsync);
        return messageBus;
    }
}