using DDDOnlineRetailerCSharp.Domain;
using DDDOnlineRetailerCSharp.Link.Services;
using DDDOnlineRetailerCSharp.Test.Fixtures;
using FluentAssertions;

namespace DDDOnlineRetailerCSharp.Test.Unit;

public class UnitTestHandlers(EventFixture eventFixture) : IClassFixture<EventFixture>
{
    [Fact]
    public async Task TestAddBatchForNewProduct()
    {
        await eventFixture.ResetDbContext();
        string? reference = "b1";

        string sku = "CRUNCHY-ARMCHAIR";
        await eventFixture.MessageBus.HandleAsync(new BatchCreated(reference, sku, 100));


        Product? got = await eventFixture.UnitOfWork.Repository.GetAsync(sku);
        Assert.NotNull(got);
        Assert.NotNull(got.Batches);
        got.Batches.Count.Should().Be(1);

        got.Batches.First().Reference.Should().Be(reference);
    }

    [Fact]
    public async Task TestAddBatchForExistingProduct()
    {
        await eventFixture.ResetDbContext();
        string? reference = "b2";

        string sku = "CRUNCHY-ARMCHAIR";
        await eventFixture.MessageBus.HandleAsync(new BatchCreated("b1", sku, 100));
        await eventFixture.MessageBus.HandleAsync(new BatchCreated(reference, sku, 99));


        Product? got = await eventFixture.UnitOfWork.Repository.GetAsync(sku);
        Assert.NotNull(got);
        Assert.NotNull(got.Batches);
        var a = got.Batches;
        got.Batches.Count.Should().Be(2);

        got.Batches.Last().Reference.Should().Be(reference);
    }

    [Fact]
    public async Task TestAllocateReturnsAllocation()
    {
        await eventFixture.ResetDbContext();
        string? reference = "b2";

        string sku = "CRUNCHY-ARMCHAIR";
        await eventFixture.MessageBus.HandleAsync(new BatchCreated(reference, sku, 100));
        Queue<object> results = await eventFixture.MessageBus.HandleAsync(new AllocationRequired("o1", sku, 10));

        
        Assert.NotNull(results);
        results.Count.Should().BeGreaterThan(0);
        object result = results.Dequeue();
        result.Should().BeOfType<string>();
        if (result is string batchRef)
        {
            batchRef.Should().Be(reference);
        }
    }
    
    [Fact]
    public async Task TestAllocateThrowExceptionForInvalidSku()
    {
        await eventFixture.ResetDbContext();
        string? reference = "b2";

        string sku = "CRUNCHY-ARMCHAIR";
        await eventFixture.MessageBus.HandleAsync(new BatchCreated(reference, sku, 100));

        
        async Task Allocate() => await eventFixture.MessageBus.HandleAsync(new AllocationRequired("o1", "NONEXISTENTSKU", 10));
        InvalidSku exception = await Assert.ThrowsAsync<InvalidSku>(Allocate);
        exception.Message.Should().Be("Invalid sku NONEXISTENTSKU");
    }

    // [Fact]
    // public async Task TestReturnsAllocation()
    // {
    //     OrderLine line = new("order1", "COMPLICATED-LAMP", 12);
    //     Batch batch = new("b1", "COMPLICATED-LAMP", 100);
    //     await eventFixture.UnitOfWork.Repository.AddAsync(batch);
    //     await eventFixture.UnitOfWork.CommitAsync();
    //     BatchService service = new( eventFixture.UnitOfWork);
    //     string result = await service.Allocate(line);
    //     result.Should().Be("b1");
    // }
    //
    // [Fact]
    // public async Task TestErrorForInvalidSku()
    // {
    //     OrderLine line = new("o1", "NONEXISTENTSKU", 10);
    //     Batch batch = new("b1", "AREALSKU", 100);
    //     await eventFixture.UnitOfWork.Repository.AddAsync(batch);
    //     await eventFixture.UnitOfWork.CommitAsync();
    //     BatchService service = new(eventFixture.UnitOfWork);
    //
    //     async Task Allocate() => await service.Allocate(line);
    //     InvalidSku exception = await Assert.ThrowsAsync<InvalidSku>(Allocate);
    //     exception.Message.Should().Be("Invalid sku NONEXISTENTSKU");
    // }
}