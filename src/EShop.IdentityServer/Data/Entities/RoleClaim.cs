using Microsoft.AspNetCore.Identity;

namespace EShop.IdentityService.Data.Entities;

public class RoleClaim : IdentityRoleClaim<Guid>
{
    public Role Role { get; set; }
}
