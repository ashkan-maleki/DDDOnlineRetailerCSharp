﻿using DDDOnlineRetailerCSharp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDDOnlineRetailerCSharp.Persistence.Entities;

public class BatchEntityTypeConfiguration : IEntityTypeConfiguration<Batch>
{
    public void Configure(EntityTypeBuilder<Batch> builder)
    {
        builder.ToTable("batches").HasKey(b => b.Id);
        
        builder.Property<int>(o => o.Id);

        builder.Property<string>(b => b.Reference!)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .IsRequired();

        builder.Property<string>(b => b.Sku!)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .IsRequired();


        builder.Property<DateTime?>(b => b.Eta)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .IsRequired(false);

        builder.Property<int>(b => b.PurchasedQuantity)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .IsRequired();

        builder.HasMany(b => b.Allocations).WithMany();

    }
}