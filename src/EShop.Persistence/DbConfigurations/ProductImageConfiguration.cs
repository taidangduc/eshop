using EShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Persistence.DbConfigurations;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable(nameof(ProductImage));

        builder.HasKey(pi => pi.Id);
        builder.Property(pi => pi.Id)
            .ValueGeneratedOnAdd();

        builder.HasIndex("ProductId")
            .IsUnique()
            .HasFilter("\"IsMain\" = true");
            //.HasFilter("[IsMain] = 1"); // Ensures only one main image per product if using SQL Server

        builder.Property(pi => pi.ImageUrl)
            .HasMaxLength(2048);
    }
}
