using DDDOnlineRetailerCSharp.Link;
using DDDOnlineRetailerCSharp.Link.Adaptors;
using DDDOnlineRetailerCSharp.Link.Services;
using DDDOnlineRetailerCSharp.Link.Services.Commands;
using DDDOnlineRetailerCSharp.Link.Services.DomainEvents;
using DDDOnlineRetailerCSharp.Persistence;
using DDDOnlineRetailerCSharp.Persistence.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DDDOnlineRetailerCSharp.Test.Fixtures;

public class EventFixture : IDisposable
{
    public EventFixture()
    {
        

        var configuration = new ConfigurationBuilder().Build();
        var serviceProvider = new ServiceCollection()
            .AddLogging() // could also be part of AddEcho to make sure ILogger is available outside ASP.NET runtime
            .AddDbContext<RetailerDbContext>(s => s.UseSqliteDatabaseOptionsBuilder())
            .BuildServiceProvider();
        // var sut = serviceProvider.GetRequiredService<IEchoService>();
        
        
        EmailService = new EmailService();
        
        DbContext = serviceProvider.GetService<RetailerDbContext>();
        
        IDomainEventHandler domainEventHandler = new DomainEventHandler(EmailService, new Logger<DomainEventHandler>(new LoggerFactory()));
        DomainEventBus = DomainEventBusFactory.RegisterAll(domainEventHandler);
        
        // DbContext = RetailerDbContext.CreateSqliteRetailerDbContext();
        UnitOfWork = new UnitOfWork(DbContext, new Repository(DbContext), DomainEventBus);
        CommandDispatcher =  CommandDispatcherFactory.RegisterAll(new CommandHandler(UnitOfWork));
        
        
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

    public IEmailService EmailService { get; }
    public IDomainEventBus DomainEventBus { get; }
    public ICommandDispatcher CommandDispatcher { get; }
    public RetailerDbContext DbContext { get; }

    public IUnitOfWork UnitOfWork { get; }

    public void Dispose()
    {
        DbContext.Dispose();
    }
}