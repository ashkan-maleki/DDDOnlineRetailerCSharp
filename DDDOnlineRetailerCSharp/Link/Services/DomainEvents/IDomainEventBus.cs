using DDDOnlineRetailerCSharp.Domain;

namespace DDDOnlineRetailerCSharp.Link.Services.DomainEvents;

public interface IDomainEventBus
{
    void RegisterHandler<T>(Func<T, Task> handler) where T : DomainEvent;
    Task HandleAsync(DomainEvent @event);
}

// https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/generic-methods
// https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/generic-interfaces
// https://learn.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/rabbitmq-event-bus-development-test-environment?source=recommendations
// https://learn.microsoft.com/en-us/previous-versions/msp-n-p/ff649664(v=pandp.10)
// https://learn.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/integration-event-based-microservice-communications?source=recommendations
// https://learn.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/subscribe-events
// https://blog.cellenza.com/en/software-development/how-to-manage-integration-events-in-an-event-driven-architecture/
// https://devblogs.microsoft.com/cesardelatorre/domain-events-vs-integration-events-in-domain-driven-design-and-microservices-architectures/
// https://learn.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/integration-event-based-microservice-communications
// https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/domain-events-design-implementation
// https://learn.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.services.notifications.webapi.notificationsubscription?view=azure-devops-dotnet
// https://learn.microsoft.com/en-us/azure/architecture/guide/architecture-styles/event-driven
// https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/
// https://github.com/dotnet/eShop
// https://learn.microsoft.com/en-us/dotnet/architecture/cloud-native/introduce-eshoponcontainers-reference-app
// https://engincanv.github.io/ddd/domain-events/integration-events/event-driven-architecture/2023/08/05/domain-events-vs-integration-events.html
// https://devblogs.microsoft.com/cesardelatorre/domain-events-vs-integration-events-in-domain-driven-design-and-microservices-architectures/