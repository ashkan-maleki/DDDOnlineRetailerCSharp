using DDDOnlineRetailerCSharp.Domain;
using FluentAssertions;

namespace DDDOnlineRetailerCSharp.Test.Unit;

public class UnitTestProduct
{
    private DateTime Today => DateTime.Today;
    private DateTime Tomorrow => DateTime.Today.AddDays(1);
    private DateTime Later => DateTime.Today.AddDays(10);

    [Fact]
    public void TestPrefersWarehouseBatchesToShipments()
    {
        string? sku = "RETRO-CLOCK";
        Batch inStockBatch = new("in-stock-batch", sku, 100);
        Batch shipmentBatch = new("shipment-batch", sku, 100, eta: Tomorrow);
        Product product = new(sku, new List<Batch>() { inStockBatch, shipmentBatch });
        OrderLine line = new("ore-f", sku, 10);
        
        product.Allocate(line);
        
        inStockBatch.AvailableQuantity.Should().Be(90);
        shipmentBatch.AvailableQuantity.Should().Be(100);
    }
    
    [Fact]
    public void TestPrefersEarlierBatches()
    {
        string sku = "MINIMALIST-SPOON";
        Batch earliest = new("speedy-batch", sku, 100, eta: Today);
        Batch medium = new("normal-batch", sku, 100, eta: Tomorrow);
        Batch latest = new("slow-batch", sku, 100, eta: Later);
        Product product = new(sku, new List<Batch>() { earliest, medium, latest });
        OrderLine line = new("order1", sku, 10);

        product.Allocate(line);
        
        earliest.AvailableQuantity.Should().Be(90);
        medium.AvailableQuantity.Should().Be(100);
        latest.AvailableQuantity.Should().Be(100);
    }

    [Fact]
    public void TestReturnsAllocatedBatchRef()
    {
        string sku = "HIGHBROW-POSTER";
        Batch inStockBatch = new("in-stock-batch-ref", sku, 100);
        Batch shipmentBatch = new("shipment-batch-ref", sku, 100, eta: Tomorrow);
        Product product = new(sku, new List<Batch>() { inStockBatch, shipmentBatch });
        OrderLine line = new("ore-f", sku, 10);

        string allocation =  product.Allocate(line);
        allocation.Should().Be(inStockBatch.Reference);
    }
    
    [Fact]
    public void TestRecordsOutOfStockEventIfCannotAllocate()
    {
        // arrange
        string sku = "SMALL-FORK";
        Batch batch = new("batch1", sku, 10, eta: Today);
        Product product = new(sku, new List<Batch>() { batch });
        // act
        product.Allocate(new OrderLine("order1", sku, 10));
        string allocation = product.Allocate(new OrderLine("order2", sku, 1));
        // Action act = () => Domain.Domain.Allocate(new OrderLine("order2", "SMALL-FORK", 1), new[] { batch });
        //assert
        
        allocation.Should().BeNull();
        // OutOfStock outOfStock = (OutOfStock)product.Events.Last();
        // outOfStock.Sku.Should().Be(sku);
        product.HasOutOfStockEvent(sku).Should().BeTrue();


        // OutOfStock exception = Assert.Throws<OutOfStock>(act);
        //The thrown exception can be used for even more detailed assertions.
        // exception.Message.Should().Be("Out of stock for sku SMALL-FORK");
    }

    [Fact]
    public void TestIncrementsVersionNumber()
    {
        string sku = "SCANDI-PEN";
        OrderLine line = new("oref", sku, 10);
        Product product = new(sku, new List<Batch>() { new("b1", sku, 100, eta: null) }, 7);
        product.Allocate(line);
        product.VersionNumber.Should().Be(8);
    }
}