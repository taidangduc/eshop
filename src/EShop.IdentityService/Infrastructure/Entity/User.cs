using Microsoft.AspNetCore.Identity;

namespace EShop.IdentityService.Infrastructure.Entity;

public class User : IdentityUser<Guid>
{
    public IList<UserRole> UserRoles { get; set; }
    public IList<PasswordHistory> PasswordHistories { get; set; }
}
