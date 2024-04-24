using DDDOnlineRetailerCSharp.Link.Adaptors;
using DDDOnlineRetailerCSharp.Link.Services;
using DDDOnlineRetailerCSharp.Persistence;
using DDDOnlineRetailerCSharp.Persistence.Helpers;
using DDDOnlineRetailerCSharp.Persistence.Seed;
using EventHandler = DDDOnlineRetailerCSharp.Link.Services.EventHandler;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer().AddSwaggerGen();
builder.Services.AddDbContext<RetailerDbContext>(s=> s.UseSqliteDatabaseOptionsBuilder());
builder.Services.AddScoped<IEventHandler, EventHandler>();
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

IEventHandler eventHandler = builder!.Services!.BuildServiceProvider()
    .GetRequiredService<IEventHandler>();

IUnitOfWork uow = builder!.Services!.BuildServiceProvider()
    .GetRequiredService<IUnitOfWork>();


MessageBus messageBus = MessageBusFactory.RegisterAll(eventHandler, uow);
builder.Services.AddSingleton<IMessageBus>(_ => messageBus);

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