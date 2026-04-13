using Microsoft.AspNetCore.Identity;

namespace EShop.IdentityService.Entities;

public class UserClaim : IdentityUserClaim<Guid>
{
    public User User { get; set; }
}
