using EShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Persistence.DbConfigurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable(nameof(OrderItem));

        // Composite primary key (OrderId + VariantId)
        builder.HasKey(oi => new { oi.OrderId, oi.VariantId });

        builder.Property(oi => oi.OrderId)
            .IsRequired();

        builder.Property(oi => oi.VariantId)
            .IsRequired();

        builder.Property(oi => oi.Title)
            .HasMaxLength(255);

        builder.Property(oi => oi.Name)
            .HasMaxLength(255);

        builder.Property(oi => oi.Price)
          .IsRequired()
          .HasColumnType("decimal(18,2)");

        builder.Property(oi => oi.Quantity)
            .IsRequired();

        builder.Property(oi => oi.ImageUrl)
            .HasMaxLength(2048);

        // TotalPrice is a computed property; exclude from database mapping
        builder.Ignore(oi => oi.TotalPrice);

        builder.HasIndex(oi => oi.VariantId);
        builder.HasIndex(oi => oi.OrderId);
    }
}
