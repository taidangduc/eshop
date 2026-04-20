using Microsoft.AspNetCore.Identity;

namespace EShop.IdentityService.Data.Entities;

public class User : IdentityUser<Guid>
{
    public IList<UserClaim> UserClaims { get; set; }
    public IList<UserLogin> UserLogins { get; set; }
    public IList<UserToken> UserTokens { get; set; }
    public IList<UserRole> UserRoles { get; set; }
}
