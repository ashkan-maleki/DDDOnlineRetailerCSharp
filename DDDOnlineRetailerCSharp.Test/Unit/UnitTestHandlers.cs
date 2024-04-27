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


        async Task Allocate() =>
            await eventFixture.MessageBus.HandleAsync(new AllocationRequired("o1", "NONEXISTENTSKU", 10));

        InvalidSku exception = await Assert.ThrowsAsync<InvalidSku>(Allocate);
        exception.Message.Should().Be("Invalid sku NONEXISTENTSKU");
    }

    [Fact]
    public async Task TestSendsEmailOnOutOfStockError()
    {
        await eventFixture.ResetDbContext();
        string? reference = "b2";
        string sku = "POPULAR-CURTAINS";
        await eventFixture.MessageBus.HandleAsync(new BatchCreated(reference, sku, 9));
        Queue<object> results = await eventFixture.MessageBus.HandleAsync(new AllocationRequired("o1", sku, 10));

        Assert.NotNull(results);
        results.Count.Should().Be(2);
        results.Dequeue();
        object result = results.Dequeue();
        result.Should().BeOfType<string>();
        if (result is string batchRef)
        {
            batchRef.Should().Be($"{sku} is ran out of stock");
        }
    }

    [Fact]
    public async Task TestChangesAvailableQuantity()
    {
        await eventFixture.ResetDbContext();
        string? reference = "batch1";
        string sku = "ADORABLE-SETTEE";

        await eventFixture.MessageBus.HandleAsync(new BatchCreated(reference, sku, 100));
        Batch? batch = (await eventFixture.UnitOfWork.Repository.GetAsync(sku))?.Batches.FirstOrDefault();
        Assert.NotNull(batch);
        await eventFixture.MessageBus.HandleAsync(new BatchQuantityChanged(reference, 50));
        batch.AvailableQuantity.Should().Be(50);
    }

    [Fact]
    public async Task TestReallocatesIfNecessary()
    {
        await eventFixture.ResetDbContext();
        string? reference1 = "batch1";
        string? reference2 = "batch2";
        string sku = "INDIFFERENT-TABLE";

        List<Event> events =
        [
            new BatchCreated(reference1, sku, 50),
            new BatchCreated(reference2, sku, 50, DateTime.Today),
            new AllocationRequired("order1", sku, 20),
            new AllocationRequired("order2", sku, 20),
        ];

        foreach (Event @event in events)
        {
            await eventFixture.MessageBus.HandleAsync(@event);
        }

        List<Batch>? batches = (await eventFixture.UnitOfWork.Repository.GetAsync(sku))?.Batches.ToList();
        Assert.NotNull(batches);
        batches.Count.Should().Be(2);
        batches[0].AvailableQuantity.Should().Be(10);
        batches[1].AvailableQuantity.Should().Be(50);
        
        await eventFixture.MessageBus.HandleAsync(new BatchQuantityChanged(reference1, 25));
        
        batches[0].AvailableQuantity.Should().Be(5);
        batches[1].AvailableQuantity.Should().Be(30);
    }
}