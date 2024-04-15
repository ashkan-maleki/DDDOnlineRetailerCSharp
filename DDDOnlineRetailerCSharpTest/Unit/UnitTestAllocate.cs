using DDDOnlineRetailerCSharp.Domain;
using FluentAssertions;

namespace DDDOnlineRetailerCSharpTest.Unit;

public class UnitTestAllocate
{
    [SetUp]
    public void Setup()
    {
    }
    
    private DateTime Today => DateTime.Today;
    private DateTime Tomorrow => DateTime.Today.AddDays(1);
    private DateTime Later => DateTime.Today.AddDays(10);

    // [Test]
    // public void TestPrefersCurrentStockBatchesToShipments()
    // {
    //     Batch inStockBatch = new("in-stock-batch", "RETRO-CLOCK", 100);
    //     Batch shipmentBatch = new("shipment-batch", "RETRO-CLOCK", 100, eta: Tomorrow);
    //     OrderLine line = new("ore-f", "RETRO-CLOCK", 10);
    //
    //     Domain.Allocate(line, new[] { inStockBatch, shipmentBatch });
    //     
    //     inStockBatch.AvailableQuantity.Should().Be(90);
    //     shipmentBatch.AvailableQuantity.Should().Be(100);
    // }
    //
    // [Test]
    // public void TestPrefersEarlierBatches()
    // {
    //     Batch earliest = new("speedy-batch", "MINIMALIST-SPOON", 100, eta: Today);
    //     Batch medium = new("normal-batch", "MINIMALIST-SPOON", 100, eta: Tomorrow);
    //     Batch latest = new("slow-batch", "MINIMALIST-SPOON", 100, eta: Later);
    //     OrderLine line = new("order1", "MINIMALIST-SPOON", 10);
    //
    //     Domain.Allocate(line, new[] { medium, earliest, latest });
    //     
    //     earliest.AvailableQuantity.Should().Be(90);
    //     medium.AvailableQuantity.Should().Be(100);
    //     latest.AvailableQuantity.Should().Be(100);
    // }
    //
    // [Test]
    // public void TestReturnsAllocatedBatchRef()
    // {
    //     Batch inStockBatch = new("in-stock-batch-ref", "HIGHBROW-POSTER", 100);
    //     Batch shipmentBatch = new("shipment-batch-ref", "HIGHBROW-POSTER", 100, eta: Tomorrow);
    //     OrderLine line = new("ore-f", "HIGHBROW-POSTER", 10);
    //
    //     string allocation =  Domain.Allocate(line, new[] { inStockBatch, shipmentBatch });
    //     allocation.Should().Be(inStockBatch.Reference);
    // }
    //
    // [Test]
    // public void TestRaisesOutOfStockExceptionIfCannotAllocate()
    // {
    //     // arrange
    //     Batch batch = new("batch1", "SMALL-FORK", 10, eta: Today);
    //     // act
    //     Domain.Allocate(new OrderLine("order1", "SMALL-FORK", 10), new[] { batch });
    //     Action act = () => Domain.Allocate(new OrderLine("order2", "SMALL-FORK", 1), new[] { batch });
    //     //assert
    //     void TestDelegate() => Domain.Allocate(new OrderLine("order2", "SMALL-FORK", 1), new[] { batch });
    //     OutOfStock? exception = Assert.Throws<OutOfStock>(TestDelegate);
    //     //The thrown exception can be used for even more detailed assertions.
    //     Assert.NotNull(exception);
    //     if (exception != null)
    //     {
    //         exception.Message.Should().Be("Out of stock for sku SMALL-FORK");
    //     }
    //     
    // }
}