using Microsoft.AspNetCore.Identity;

namespace EShop.IdentityService.Entities;

public class User : IdentityUser<Guid>
{
    public IList<UserClaim> Claims { get; set; }
    public IList<UserToken> Tokens { get; set; }
    public IList<UserRole> UserRoles { get; set; }
    public IList<UserLogin> UserLogins { get; set; }
}
