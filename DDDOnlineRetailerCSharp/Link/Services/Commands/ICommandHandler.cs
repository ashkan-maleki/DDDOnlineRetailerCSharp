using DDDOnlineRetailerCSharp.Domain;

namespace DDDOnlineRetailerCSharp.Link.Services.Commands;

public interface ICommandHandler
{
    public Task HandleAsync(CreateBatch command);
    public Task HandleAsync(ChangeBatchQuantity command);
    public Task HandleAsync(Allocate command);

}