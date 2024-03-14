using DDDOnlineRetailerCSharp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDDOnlineRetailerCSharp.Persistence.Entities;

public class OrderLineEntityTypeConfiguration : IEntityTypeConfiguration<OrderLine>
{
    public void Configure(EntityTypeBuilder<OrderLine> builder)
    {
        builder.ToTable("order_lines").HasKey(o => o.Id);
        
        builder.Property<int>(o => o.Id);

        builder.Property<string>(o => o.OrderId!)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .IsRequired();

        builder.Property<string>(b => b.Sku!)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .IsRequired();

        builder.Property<int>(b => b.Qty)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .IsRequired();
    }
}