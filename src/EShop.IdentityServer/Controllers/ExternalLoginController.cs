using System.Security.Claims;
using EShop.Contracts.Customer.DTOs;
using EShop.Contracts.Customer.Services;
using EShop.IdentityService.Authorization;
using EShop.IdentityService.Entities;
using EShop.IdentityService.Models.ExternalLogin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EShop.IdentityService.Controllers;

public class ExternalLoginController : Controller
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly ICustomerService _customerService;

    public ExternalLoginController(
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        ICustomerService customerService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _customerService = customerService;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Challenge(ExternalLoginModel model)
    {
        var redirectUrl = Url.Action(nameof(Callback), "ExternalLogin", new { model.ReturnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(model.Provider, redirectUrl);
        return base.Challenge(properties, model.Provider);
    }

    [HttpGet]
    public async Task<IActionResult> Callback(string? returnUrl = null, string? remoteError = null)
    {
        // Handle errors returned by the external provider
        if (remoteError is not null)
        {
            TempData["Error"] = $"Error from external provider: {remoteError}";

            return RedirectToAction("Login", "Account", new { returnUrl });
        }

        // Retrieve the external login info stored in the external cookie by the provider
        var info = await _signInManager.GetExternalLoginInfoAsync();

        if (info is null)
        {
            TempData["Error"] = "Error loading external login information.";

            return RedirectToAction("Login", "Account", new { returnUrl });
        }

        // Check if the external login is already linked to a local account
        var IsConnected = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

        if (IsConnected.Succeeded)
        {
            return RedirectToLocal(returnUrl);
        }

        // Retrieve claims from the external providers
        var email = info.Principal.FindFirstValue(ClaimTypes.Email);

        if (string.IsNullOrEmpty(email))
        {
            TempData["Error"] = "Email claim not received from external provider.";

            return RedirectToAction("Login", "Account", new { returnUrl });
        }

        // Check account already exists by email
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            // Set default username
            var userName = GenerateRandomText();

            // Create a local account without password
            user = new User
            {
                UserName = userName,
                Email = email,
                EmailConfirmed = true,
            };

            var createResult = await _userManager.CreateAsync(user);

            if (!createResult.Succeeded)
            {
                TempData["Error"] = "Failed to create account from external login.";

                return RedirectToAction("Login", "Account", new { returnUrl });
            }

            // Assign default role
            await _userManager.AddToRoleAsync(user, Roles.User);
         
            try
            {
                // Call service to service (HTTP) create Customer in Business Service
                await _customerService.CreateAsync(new CreateCustomerModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                });
            }
            catch
            {
                // Rollback
                await _userManager.DeleteAsync(user);
                TempData["Error"] = "Error occurred when creating the account.";

                return RedirectToAction("Login", "Account", new { returnUrl });
            }
        }

        // Add the external login info to the local account
        var localLogin = await _userManager.AddLoginAsync(user, info);

        if (!localLogin.Succeeded)
        {
            TempData["Error"] = "Failed to link external login to account.";

            return RedirectToAction("Login", "Account", new { returnUrl });
        }

        // Sign the user in with the local account
        await _signInManager.SignInAsync(user, isPersistent: false);

        return RedirectToLocal(returnUrl);
    }

    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    private static string GenerateRandomText()
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Range(0, 10)
            .Select(_ => chars[Random.Shared.Next(chars.Length)])
            .ToArray());
    }
}

