using EShop.Contracts.IntegrationEvents;
using EShop.IdentityService.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EShop.Migrator;
using EShop.Infrastructure.Identity;
using EShop.EventBus;

namespace EShop.IdentityService.Persistence.Seed;

public class IdentityDataSeeder : IDataSeeder<IdentityDbContext>
{
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly IEventBus _eventBus;
    public IdentityDataSeeder(
        RoleManager<Role> roleManager,
        UserManager<User> userManager,
        IEventBus eventBus)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _eventBus = eventBus;
    }
    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        await SeedRoles();
        await SeedUsers();
    }

    public async Task SeedRoles()
    {
        if (!await _roleManager.Roles.AnyAsync())
        {
            if (!await _roleManager.RoleExistsAsync(Authorization.Roles.Admin))
            {
                await _roleManager.CreateAsync(new Role { Name = Authorization.Roles.Admin });
            }

            if (!await _roleManager.RoleExistsAsync(Authorization.Roles.User))
            {
                await _roleManager.CreateAsync(new Role { Name = Authorization.Roles.User });
            }
        }
    }

    public async Task SeedUsers()
    {
        if (!await _userManager.Users.AnyAsync())
        {
            if (await _userManager.FindByNameAsync("peter") == null)
            {
                var result = await _userManager.CreateAsync(IdentityData.Users.First(), "admin@12345");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(IdentityData.Users.First(), Authorization.Roles.Admin);
                    await _eventBus.SendAsync(new UserCreatedEvent
                    {
                        UserId = IdentityData.Users.First().Id,
                        Email = IdentityData.Users.First().Email!
                    });
                }
            }

            if (await _userManager.FindByNameAsync("mira") == null)
            {
                var result = await _userManager.CreateAsync(IdentityData.Users.Last(), "user@12345");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(IdentityData.Users.Last(), Authorization.Roles.User);
                    await _eventBus.SendAsync(new UserCreatedEvent
                    {
                        UserId = IdentityData.Users.Last().Id,
                        Email = IdentityData.Users.Last().Email!
                    });
                }
            }
        }
    }
}