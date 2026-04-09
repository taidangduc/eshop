using EShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Persistence.DbConfigurations;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable(nameof(OutboxMessage));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Payload)
            .IsRequired();

        builder.Property(x => x.EventType)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Published)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.ScheduledAt)
            .IsRequired();

        builder.HasIndex(x => x.Published);
        builder.HasIndex(x => x.ScheduledAt);
    }
}
