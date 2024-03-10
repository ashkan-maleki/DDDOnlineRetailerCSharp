using DDDOnlineRetailerCSharp.Domain;
using FluentAssertions;

namespace DDDOnlineRetailerCSharpTest.Unit;



public class UnitTestBatches
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestAllocatingToABatchReducesTheAvailableQuantity()
    {
        Batch batch = new("batch-001", "SMALL-TABLE", 20, eta: DateTime.Today);
        OrderLine ol = new("order-ref", "SMALL-TABLE", 2);


        batch.CanAllocate(ol).Should().BeTrue();
        batch.Allocate(ol);
        batch.AvailableQuantity.Should().Be(18);
    }

    private record BatchAndLine(Batch Batch, OrderLine OrderLine);

    private (Batch, OrderLine) MakeBatchAndLine(string sku, int batchQty, int lineQty) =>
        (new("batch-001", sku, batchQty, eta: DateTime.Today),
            new("order-123", sku, lineQty));

    [Test]
    public void TestCanAllocateIfAvailableGreaterThanRequired()
    {
        (Batch batch, OrderLine ol) = MakeBatchAndLine("ELEGANT-LAMP", 20, 2);
        batch.CanAllocate(ol).Should().BeTrue();
    }
    
    [Test]
    public void TestCannotAllocateIfAvailableSmallThanRequired()
    {
        (Batch batch, OrderLine ol) = MakeBatchAndLine("ELEGANT-LAMP", 2, 20);
        batch.CanAllocate(ol).Should().BeFalse();
    }

    [Test]
    public void TestCanAllocateIfAvailableEqualToRequired()
    {
        (Batch batch, OrderLine ol) = MakeBatchAndLine("ELEGANT-LAMP", 2, 2);
        batch.CanAllocate(ol).Should().BeTrue();
    }
    
    [Test]
    public void TestCannotAllocateIfSkusDoNotMatch()
    {
        Batch batch = new("batch-001", "UNCOMFORTABLE-CHAIR", 100, eta: null);
        OrderLine differentSkuLine = new("order-123", "EXPENSIVE-TOASTER", 10);
        batch.CanAllocate(differentSkuLine).Should().BeFalse();
    }
    
    [Test]
    public void TestCanOnlyDeallocateAllocatedLines()
    {
        (Batch batch, OrderLine unallocatedLine) = MakeBatchAndLine("DECORATIVE-TRINKET", 20, 2);
        batch.Deallocate(unallocatedLine);
        batch.AvailableQuantity.Should().Be(20);
    }
}