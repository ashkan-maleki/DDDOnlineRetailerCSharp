using DDDOnlineRetailerCSharp.Domain;

namespace DDDOnlineRetailerCSharp.Link.Services.Commands;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly Dictionary<Type, Func<Command, Task>> _handlers = new();
    public void RegisterHandler<T>(Func<T, Task> handler) where T : Command
    {
        if (_handlers.ContainsKey(typeof(T)))
        {
            throw new IndexOutOfRangeException("You cannot register the same command handler twice");
        }

        _handlers.Add(typeof(T), x => handler((T)x));
    }

    public async Task HandleAsync(Command command)
    {
        if (_handlers.TryGetValue(command.GetType(), out Func<Command, Task>? handler))
        {
            await handler(command);
        }
        else
        {
            throw new ArgumentNullException(nameof(handler), "No event handler was registered");
        }
    }
}