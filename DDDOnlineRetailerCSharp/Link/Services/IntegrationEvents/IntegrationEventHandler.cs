using DDDOnlineRetailerCSharp.Application;
using DDDOnlineRetailerCSharp.Domain;
using DDDOnlineRetailerCSharp.EventBus;
using DDDOnlineRetailerCSharp.Link.Adaptors;
using DDDOnlineRetailerCSharp.Persistence;

namespace DDDOnlineRetailerCSharp.Link.Services.IntegrationEvents;

public interface IGenericEventHandler<in TEvent> where TEvent : Event
{
    Task HandleAsync(TEvent @event);
}

public interface IOutOfStockHandler : IGenericEventHandler<OutOfStockIntegrationEvent>
{
    new Task HandleAsync(OutOfStockIntegrationEvent @event);

    Task IGenericEventHandler<OutOfStockIntegrationEvent>.HandleAsync(OutOfStockIntegrationEvent @event) =>
        HandleAsync(@event);
}

public class IntegrationEventHandler(IEmailService emailService, RetailerDbContext dbContext, ILogger<IntegrationEventHandler> logger) : IIntegrationEventHandler
{
    public Task HandleAsync(OutOfStockIntegrationEvent @event)
    {
        emailService.Send("admin@eshop.com", $"{@event.Sku} is ran out of stock");
        logger.LogInformation("Published integration event: {EventID} - ({@EventType})", @event.Id, typeof(OutOfStockIntegrationEvent));
        return Task.FromResult(0);
    }

    public async Task HandleAsync(DeallocatedIntegrationEvent @event)
    {
        dbContext.Allocations.RemoveRange(dbContext.Allocations.Where(
            allocation => allocation.Sku == @event.Sku && allocation.OrderId == @event.OrderId && allocation.BatchRef == @event.Reference));
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Published integration event: {EventID} - ({@EventType})", @event.Id, typeof(DeallocatedIntegrationEvent));
    }


    public async Task HandleAsync(AllocatedIntegrationEvent @event)
    {
        await dbContext.Allocations.AddAsync(new AllocationView(@event.Sku, @event.OrderId, @event.Reference));
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Published integration event: {EventID} - ({@EventType})", @event.Id, typeof(AllocatedIntegrationEvent));
    }
}