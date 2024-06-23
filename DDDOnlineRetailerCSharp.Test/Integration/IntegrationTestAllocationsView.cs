using DDDOnlineRetailerCSharp.Domain;
using DDDOnlineRetailerCSharp.Persistence;
using DDDOnlineRetailerCSharp.Test.Fixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DDDOnlineRetailerCSharp.Test.Integration;

public class IntegrationTestAllocationsView(EventFixture eventFixture) : IClassFixture<EventFixture>
{
    [Fact]
    public async Task TestAllocationsView()
    {
        await eventFixture.CommandDispatcher.HandleAsync(new CreateBatch("sku1batch", "sku1", 50));
        await eventFixture.CommandDispatcher.HandleAsync(new CreateBatch("sku2batch", "sku2", 50, DateTime.Now));
        
        await eventFixture.CommandDispatcher.HandleAsync(new Allocate("order1", "sku1", 20));
        await eventFixture.CommandDispatcher.HandleAsync(new Allocate("order1", "sku2", 20));
        
        await eventFixture.CommandDispatcher.HandleAsync(new CreateBatch("sku1batch-later", "sku1", 50, DateTime.Now));
        
        await eventFixture.CommandDispatcher.HandleAsync(new Allocate("otherorder", "sku1", 30));
        await eventFixture.CommandDispatcher.HandleAsync(new Allocate("otherorder", "sku2", 10));


        List<AllocationView> allocated = await eventFixture.DbContext.Allocations.Where(allocate => allocate.OrderId == "order1").ToListAsync();
        allocated.Should().HaveCount(2);
        allocated.Exists(al => al.Sku == "sku1" && al.BatchRef == "sku1batch").Should().BeTrue();
        allocated.Exists(al => al.Sku == "sku2" && al.BatchRef == "sku2batch").Should().BeTrue();
    }
    
    [Fact]
    public async Task TestDeallocation()
    {
        await eventFixture.CommandDispatcher.HandleAsync(new CreateBatch("b1", "sku1", 50));
        await eventFixture.CommandDispatcher.HandleAsync(new CreateBatch("b2", "sku1", 50, DateTime.Now));
        
        await eventFixture.CommandDispatcher.HandleAsync(new Allocate("o1", "sku1", 40));
        
        await eventFixture.CommandDispatcher.HandleAsync(new ChangeBatchQuantity("b1", 10));


        List<AllocationView> allocated = await eventFixture.DbContext.Allocations.Where(allocate => allocate.OrderId == "o1").ToListAsync();
        allocated.Should().HaveCount(1);
        allocated.Exists(al => al.Sku == "sku1" && al.BatchRef == "b2").Should().BeTrue();
    }
}