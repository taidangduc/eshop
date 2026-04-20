using EShop.IdentityService.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.IdentityService.Data;

public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.ToTable("UserTokens");

        builder.HasOne(x => x.User)
         .WithMany(x => x.UserTokens)
         .HasForeignKey(x => x.UserId)
         .IsRequired();
    }
}
