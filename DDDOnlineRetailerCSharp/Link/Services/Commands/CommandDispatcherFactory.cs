using DDDOnlineRetailerCSharp.Domain;

namespace DDDOnlineRetailerCSharp.Link.Services.Commands;

public static class CommandDispatcherFactory
{
    public static CommandDispatcher RegisterAll(ICommandHandler handler)
    {
        CommandDispatcher commandDispatcher = new();
        commandDispatcher.RegisterHandler<CreateBatch>(handler.HandleAsync);
        commandDispatcher.RegisterHandler<ChangeBatchQuantity>(handler.HandleAsync);
        commandDispatcher.RegisterHandler<Allocate>(handler.HandleAsync);
        return commandDispatcher;
    }
}