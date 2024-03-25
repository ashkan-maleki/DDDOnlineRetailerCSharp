using DDDOnlineRetailerCSharp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDDOnlineRetailerCSharp.Persistence.Entities;

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products").HasKey(o => o.Sku);
        
        builder.Property(o => o.Sku)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Property(o => o.VersionNumber)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .IsConcurrencyToken()
            .IsRequired();

        builder.HasMany(product => product.Batches)
            .WithOne()
            .HasForeignKey(batch => batch.Sku)
            .IsRequired();
    }
}