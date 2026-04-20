using Microsoft.AspNetCore.Identity;

namespace EShop.IdentityService.Data.Entities;

public class UserClaim : IdentityUserClaim<Guid>
{
    public User User { get; set; }
}
