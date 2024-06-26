﻿using DDDOnlineRetailerCSharp.Domain;
using DDDOnlineRetailerCSharp.Persistence;
using DDDOnlineRetailerCSharp.Test.Fixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DDDOnlineRetailerCSharp.Test.Integration;

public class IntegrationTestRepository(EventFixture eventFixture, DataFixture dataFixture)
    : IClassFixture<EventFixture>, IClassFixture<DataFixture>
{
    [Fact]
    public async Task TestRepositoryCanSaveAProduct()
    {
        string? sku = "RUSTY-SOAPDISH";

        Batch batch = new("batch1", sku, 100);
        Product product = new(sku, new List<Batch>() { batch });


        await eventFixture.UnitOfWork.Repository.AddAsync(product: product);
        await eventFixture.UnitOfWork.CommitAsync();

        Batch got = await eventFixture.DbContext.Batches.FromSql($"SELECT * FROM batches").FirstAsync();
        got.Should().Be(batch);
    }

    [Fact]
    public async Task TestRepositoryCanRetrieveABatchWithAllocations()
    {
        RetailerDbContext dbContext = eventFixture.DbContext;

        await dataFixture.InsertProduct(dbContext);
        string reference = "batch1";
        int batch1Id = await dataFixture.InsertBatch(dbContext, reference);
        await dataFixture.InsertBatch(dbContext, "batch2");
        int orderLineId = await dataFixture.InsertOrderLine(dbContext);
        await dataFixture.InsertAllocation(dbContext, batch1Id, orderLineId);

        Product? product = await eventFixture.UnitOfWork.Repository.GetAsync(dataFixture.GetSku);
        Batch got = product.Batches.First(batch => batch.Reference == reference);
        Batch expected = new(reference, dataFixture.GetSku, 100);
        List<OrderLine> expectedLine = new() { new("order1", dataFixture.GetSku, 12) };


        expected.Should().Be(got);
        expected.Sku.Should().Be(got.Sku);
        expected.PurchasedQuantity.Should().Be(got.PurchasedQuantity);
        expectedLine.Should().Equal(got.Allocations);
    }
}