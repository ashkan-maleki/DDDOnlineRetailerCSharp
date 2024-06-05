using DDDOnlineRetailerCSharp.Domain;
using DDDOnlineRetailerCSharp.Link.Adaptors;
using DDDOnlineRetailerCSharp.Link.Services.DomainEvents;

namespace DDDOnlineRetailerCSharp.Link.Services.IntegrationEvents;


public interface IGenericEventHandler<in TEvent> where TEvent : Event
{
    Task HandleAsync(TEvent @event) ;
}


public interface IOutOfStockHandler : IGenericEventHandler<OutOfStock>
{
    new Task HandleAsync(OutOfStock @event);
    Task IGenericEventHandler<OutOfStock>.HandleAsync(OutOfStock @event) => HandleAsync(@event);

}


public class IntegrationDomainEventHandler(IEmailService emailService, IUnitOfWork uow, ILogger<IntegrationDomainEventHandler> logger) : IIntegrationEventHandler
{
    public Task HandleAsync(OutOfStock @event) => Task.FromResult((object)emailService.Send($"{@event.Sku} is ran out of stock"));

    public async Task HandleAsync(BatchCreated @event)
    {
        logger.LogInformation("Publishing integration event: {EventID} - ({@EventType})", @event.Id, typeof(BatchCreated));
        await Task.FromResult(0);
    }

    public async Task HandleAsync(BatchQuantityChanged @event)
    {
        logger.LogInformation("Publishing integration event: {EventID} - ({@EventType})", @event.Id, typeof(BatchQuantityChanged));
        await Task.FromResult(0);
    }

    public async Task HandleAsync(Deallocated @event)
    {
        logger.LogInformation("Publishing integration event: {EventID} - ({@EventType})", @event.Id, typeof(Deallocated));
        await Task.FromResult(0);
    }
}