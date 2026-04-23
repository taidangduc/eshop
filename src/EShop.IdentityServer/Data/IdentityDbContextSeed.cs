using EShop.Contracts.Customer.DTOs;
using EShop.Contracts.Customer.Services;
using EShop.IdentityService.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EShop.IdentityService.Data;

public class IdentityDataContextSeed
{
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly ICustomerService _customerService;
    private readonly IEnumerable<User> _users =
    [
       new()
        {
            Id = Guid.NewGuid(),
            UserName = "peter",
            Email = "peter@test.com",
            SecurityStamp = Guid.NewGuid().ToString()
        },
        new()
        {
            Id = Guid.NewGuid(),
            UserName = "mira",
            Email = "mira@test.com",
            SecurityStamp = Guid.NewGuid().ToString()
        }
    ];
    public IdentityDataContextSeed(
        RoleManager<Role> roleManager,
        UserManager<User> userManager,
        ICustomerService customerService)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _customerService = customerService;
    }
    public async Task SeedAsync()
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
                var result = await _userManager.CreateAsync(_users.First(), "admin@12345");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(_users.First(), Authorization.Roles.Admin);
                    await _customerService.CreateAsync(new CreateCustomerModel
                    {
                        UserId = _users.First().Id,
                        Email = _users.First().Email!,
                    });
                }
            }

            if (await _userManager.FindByNameAsync("mira") == null)
            {
                var result = await _userManager.CreateAsync(_users.Last(), "user@12345");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(_users.Last(), Authorization.Roles.User);
                    await _customerService.CreateAsync(new CreateCustomerModel
                    {
                        UserId = _users.Last().Id,
                        Email = _users.Last().Email!
                    });
                }
            }
        }
    }
}