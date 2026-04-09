using EShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Persistence.DbConfigurations;

public class FileEntryConfiguration : IEntityTypeConfiguration<FileEntry>
{
    public void Configure(EntityTypeBuilder<FileEntry> builder)
    {
        builder.ToTable("FileEntries");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.FileName).IsRequired().HasMaxLength(255);
        builder.Property(x => x.FileLocation).IsRequired().HasMaxLength(1000);
        builder.Property(x => x.ContentType).IsRequired().HasMaxLength(100);
    }
}
