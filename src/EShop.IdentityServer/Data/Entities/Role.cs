using Microsoft.AspNetCore.Identity;

namespace EShop.IdentityService.Data.Entities;

public class Role : IdentityRole<Guid>
{
    public IList<RoleClaim> RoleClaims { get; set; }
    public IList<UserRole> UserRoles { get; set; }
}
