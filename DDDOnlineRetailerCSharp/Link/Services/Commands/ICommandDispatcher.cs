using DDDOnlineRetailerCSharp.Domain;

namespace DDDOnlineRetailerCSharp.Link.Services.Commands;

public interface ICommandDispatcher
{
    void RegisterHandler<T>(Func<T, Task> handler) where T : Command;
    Task HandleAsync(Command command);
}