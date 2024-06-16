using DDDOnlineRetailerCSharp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDDOnlineRetailerCSharp.Persistence.Entities;

public class AllocationViewTypeConfiguration : IEntityTypeConfiguration<AllocationView>
{
    public void Configure(EntityTypeBuilder<AllocationView> builder)
    {
        builder.ToTable("allocations_view");

        builder.Property<string>(b => b.OrderId!)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .IsRequired();

        builder.Property<string>(b => b.Sku!)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .IsRequired();


        builder.Property<string?>(b => b.BatchRef)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .IsRequired(false);

    }

    
}