using Microsoft.AspNetCore.Identity;

namespace EShop.IdentityService.Data.Entities;

public class UserToken : IdentityUserToken<Guid>
{
	public User User { get; set; }
}
