using DDDOnlineRetailerCSharp.Link.Adaptors;
using DDDOnlineRetailerCSharp.Link.Services;
using DDDOnlineRetailerCSharp.Link.Services.Commands;
using DDDOnlineRetailerCSharp.Link.Services.DomainEvents;
using DDDOnlineRetailerCSharp.Persistence;
using DDDOnlineRetailerCSharp.Persistence.Helpers;
using DDDOnlineRetailerCSharp.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer().AddSwaggerGen();
builder.Services.AddDbContext<RetailerDbContext>(s=> s.UseSqliteDatabaseOptionsBuilder());
builder.Services.AddScoped<ICommandHandler, CommandHandler>();
builder.Services.AddScoped<IDomainEventHandler, DomainEventHandler>();
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

ICommandHandler commandHandler = builder!.Services!.BuildServiceProvider()
    .GetRequiredService<ICommandHandler>();
CommandDispatcher commandDispatcher = CommandDispatcherFactory.RegisterAll(commandHandler);
builder.Services.AddSingleton<ICommandDispatcher>(_ => commandDispatcher);


IDomainEventHandler domainEventHandler = builder!.Services!.BuildServiceProvider()
    .GetRequiredService<IDomainEventHandler>();
// IUnitOfWork uow = builder!.Services!.BuildServiceProvider()
//     .GetRequiredService<IUnitOfWork>();
DomainDomainEventBus domainDomainEventBus = DomainEventBusFactory.RegisterAll(domainEventHandler);
builder.Services.AddSingleton<IDomainEventBus>(_ => domainDomainEventBus);

var app = builder.Build();

using  var scope = app.Services.CreateScope();
RetailerDbContext? dbContext = scope.ServiceProvider.GetService<RetailerDbContext>();
FakeFactory.CreateBatch(dbContext!);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();