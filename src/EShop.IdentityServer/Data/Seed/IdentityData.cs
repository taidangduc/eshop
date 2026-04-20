using EShop.IdentityService.Data.Entities;

namespace EShop.IdentityService.Data.Seed;

public static class IdentityData
{
    public static List<User> Users { get; set; }

    static IdentityData()
    {
        Users = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid(),
                UserName = "peter",
                Email = "peter@test.com",
                SecurityStamp = Guid.NewGuid().ToString()
            },
            new User
            {
                Id = Guid.NewGuid(),
                UserName = "mira",
                Email = "mira@test.com",
                SecurityStamp = Guid.NewGuid().ToString()
            },
        };
    }
}


