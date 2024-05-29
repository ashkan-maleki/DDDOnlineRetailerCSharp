using DDDOnlineRetailerCSharp.Link;
using DDDOnlineRetailerCSharp.Link.Adaptors;
using DDDOnlineRetailerCSharp.Link.Services;
using DDDOnlineRetailerCSharp.Link.Services.Events;
using DDDOnlineRetailerCSharp.Persistence;
using Microsoft.EntityFrameworkCore;
using EventHandler = DDDOnlineRetailerCSharp.Link.Services.Events.EventHandler;

namespace DDDOnlineRetailerCSharp.Test.Fixtures;

public class EventFixture : IDisposable
{
    public EventFixture()
    {
        DbContext = RetailerDbContext.CreateSqliteRetailerDbContext();
        
        UnitOfWork = new UnitOfWork(DbContext, new Repository(DbContext));
        IEmailService emailService = new EmailService();
        IEventHandler eventHandler = new EventHandler(emailService, UnitOfWork);
        EventBus = EventBusFactory.RegisterAll(eventHandler, UnitOfWork);
    }

    public async Task ResetDbContext()
    {
        // await DbContext.Database.EnsureDeletedAsync();
        // await DbContext.Database.EnsureCreatedAsync();
        DbContext.Products.RemoveRange(DbContext.Products);
        DbContext.Batches.RemoveRange(DbContext.Batches);
        DbContext.OrderLines.RemoveRange(DbContext.OrderLines);
        await DbContext.SaveChangesAsync();
    }
    public IEventBus EventBus { get; }
    public RetailerDbContext DbContext { get; }

    public IUnitOfWork UnitOfWork { get; }

    public void Dispose()
    {
        DbContext.Dispose();
    }
}