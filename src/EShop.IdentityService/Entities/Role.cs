using Microsoft.AspNetCore.Identity;

namespace EShop.IdentityService.Entities;

public class Role : IdentityRole<Guid>
{
    public IList<RoleClaim> RoleClaims { get; set; }
    public IList<UserRole> UserRoles { get; set; }
}
