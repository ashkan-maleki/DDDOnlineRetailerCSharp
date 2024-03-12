using DDDOnlineRetailerCSharp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDDOnlineRetailerCSharp.Persistence.Entities;

public class BatchEntityTypeConfiguration : IEntityTypeConfiguration<Batch>
{
    public void Configure(EntityTypeBuilder<Batch> builder)
    {
        builder.ToTable("batches");

        builder.Property<string>("_reference")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("Reference")
            .IsRequired();
        
        builder.Property<string>("_sku")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("SKU")
            .IsRequired();
        
        
        builder.Property<DateTime?>("_eta")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("ETA")
            .IsRequired(false);
        
        builder.Property<int>("_purchasedQuantity")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("PurchasedQuantity")
            .IsRequired();

    }
}