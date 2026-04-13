using Microsoft.AspNetCore.Identity;

namespace EShop.IdentityService.Entities;

public class UserToken : IdentityUserToken<Guid>
{
	public User User { get; set; }
}
