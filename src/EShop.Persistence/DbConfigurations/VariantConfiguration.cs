using EShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Persistence.DbConfigurations;

public class VariantConfiguration : IEntityTypeConfiguration<Variant>
{
    public void Configure(EntityTypeBuilder<Variant> builder)
    {
        builder.ToTable(nameof(Variant));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.ProductId)
            .IsRequired();

        builder.Property(x => x.Title)
            .HasMaxLength(255);

        builder.Property(x => x.Name)
            .HasMaxLength(255);

        builder.Property(x => x.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.Property(x => x.Sku)
            .HasMaxLength(100);

        builder.Property(x => x.ImageUrl)
            .HasMaxLength(2048);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.HasMany(x => x.OptionValues)
            .WithOne()
            .HasForeignKey(vov => vov.VariantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
