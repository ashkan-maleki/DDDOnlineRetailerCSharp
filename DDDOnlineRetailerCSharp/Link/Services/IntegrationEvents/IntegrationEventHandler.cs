using DDDOnlineRetailerCSharp.Application;
using DDDOnlineRetailerCSharp.EventBus;
using DDDOnlineRetailerCSharp.Link.Adaptors;

namespace DDDOnlineRetailerCSharp.Link.Services.IntegrationEvents;


public interface IGenericEventHandler<in TEvent> where TEvent : Event
{
    Task HandleAsync(TEvent @event) ;
}


public interface IOutOfStockHandler : IGenericEventHandler<OutOfStockIntegrationEvent>
{
    new Task HandleAsync(OutOfStockIntegrationEvent @event);
    Task IGenericEventHandler<OutOfStockIntegrationEvent>.HandleAsync(OutOfStockIntegrationEvent @event) => HandleAsync(@event);

}


public class IntegrationEventHandler(IEmailService emailService) : IIntegrationEventHandler
{
    public Task HandleAsync(OutOfStockIntegrationEvent @event)
    {
        emailService.Send("admin@eshop.com", $"{@event.Sku} is ran out of stock");
        return Task.FromResult(0);
    }
}