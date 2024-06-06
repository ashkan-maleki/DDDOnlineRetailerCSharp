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
        await eventFixture.CommandDispatcher.HandleAsync(new CreateBatch(reference, sku, 100));

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

        string sku = "COMPLICATED-LAMP";
        await eventFixture.CommandDispatcher.HandleAsync(new CreateBatch("b1", sku, 100));
        await eventFixture.CommandDispatcher.HandleAsync(new CreateBatch(reference, sku, 99));

        Product? got = await eventFixture.UnitOfWork.Repository.GetAsync(sku);
        Assert.NotNull(got);
        Assert.NotNull(got.Batches);
        
        got.Batches.Count.Should().Be(2);
        got.Batches.Last().Reference.Should().Be(reference);
    }

    [Fact]
    public async Task TestAllocate()
    {
        await eventFixture.ResetDbContext();
        string? reference = "b2";
    
        string sku = "CRUNCHY-ARMCHAIR";
        await eventFixture.CommandDispatcher.HandleAsync(new CreateBatch(reference, sku, 100));
        await eventFixture.CommandDispatcher.HandleAsync(new Allocate("o1", sku, 10));

        Product? product = await eventFixture.UnitOfWork.Repository.GetAsync(sku);
        product.Should().NotBeNull();
        product!.Batches.Should().HaveCount(1);
        Batch batch = product.Batches.FirstOrDefault()!;
        batch.AvailableQuantity.Should().Be(90);
    }

    [Fact]
    public async Task TestAllocateThrowExceptionForInvalidSku()
    {
        await eventFixture.ResetDbContext();
        string? reference = "b2";

        string sku = "CRUNCHY-ARMCHAIR";
        await eventFixture.CommandDispatcher.HandleAsync(new CreateBatch(reference, sku, 100));


        async Task Allocate() =>
            await eventFixture.CommandDispatcher.HandleAsync(new Allocate("o1", "NONEXISTENTSKU", 10));

        InvalidSku exception = await Assert.ThrowsAsync<InvalidSku>(Allocate);
        exception.Message.Should().Be("Invalid sku NONEXISTENTSKU");
    }

    [Fact]
    public async Task TestSendsEmailOnOutOfStockError()
    {
        await eventFixture.ResetDbContext();
        string? reference = "b2";
        string sku = "POPULAR-CURTAINS";
        await eventFixture.CommandDispatcher.HandleAsync(new CreateBatch(reference, sku, 9));
        await eventFixture.CommandDispatcher.HandleAsync(new Allocate("o1", sku, 10));
        eventFixture.EmailService.Sent("admin@eshop.com").Should().BeTrue();
    }

    [Fact]
    public async Task TestChangesAvailableQuantity()
    {
        await eventFixture.ResetDbContext();
        string? reference = "batch1";
        string sku = "ADORABLE-SETTEE";

        await eventFixture.CommandDispatcher.HandleAsync(new CreateBatch(reference, sku, 100));
        Batch? batch = (await eventFixture.UnitOfWork.Repository.GetAsync(sku))?.Batches.FirstOrDefault();
        Assert.NotNull(batch);
        await eventFixture.CommandDispatcher.HandleAsync(new ChangeBatchQuantity(reference, 50));
        batch.AvailableQuantity.Should().Be(50);
    }

    [Fact]
    public async Task TestReallocatesIfNecessary()
    {
        await eventFixture.ResetDbContext();
        string? reference1 = "batch1";
        string? reference2 = "batch2";
        string sku = "INDIFFERENT-TABLE";

        List<Command> commands =
        [
            new CreateBatch(reference1, sku, 50),
            new CreateBatch(reference2, sku, 50, DateTime.Today),
            new Allocate("order1", sku, 20),
            new Allocate("order2", sku, 20),
        ];

        foreach (Command command in commands)
        {
            await eventFixture.CommandDispatcher.HandleAsync(command);
        }

        List<Batch>? batches = (await eventFixture.UnitOfWork.Repository.GetAsync(sku))?.Batches.ToList();
        Assert.NotNull(batches);
        batches.Count.Should().Be(2);
        batches[0].AvailableQuantity.Should().Be(10);
        batches[1].AvailableQuantity.Should().Be(50);
        
        await eventFixture.CommandDispatcher.HandleAsync(new ChangeBatchQuantity(reference1, 25));
        
        batches[0].AvailableQuantity.Should().Be(5);
        batches[1].AvailableQuantity.Should().Be(30);
    }
}


// https://kaylumah.nl/2021/11/14/capture-logs-in-unit-tests.html