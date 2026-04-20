using Microsoft.AspNetCore.Identity;

namespace EShop.IdentityService.Data.Entities;

public class UserLogin : IdentityUserLogin<Guid>
{
    public User User { get; set; }
}
