using EShop.IdentityService.Entities;
using EShop.IdentityService.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Duende.IdentityServer.Services;
using EShop.Contracts.IntegrationEvents;
using EShop.EventBus;

namespace EShop.IdentityService.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IEventBus _eventBus;

    public AccountController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IIdentityServerInteractionService interaction,
        IEventBus eventBus)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _interaction = interaction;
        _eventBus = eventBus;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string returnUrl = null)
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToLocal(returnUrl);
        }

        Console.WriteLine($"Login GET called {returnUrl}");

        ViewData["ReturnUrl"] = returnUrl;

        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginModel model)
    {
        ViewData["ReturnUrl"] = model.ReturnUrl;

        Console.WriteLine($"Login POST called {model.ReturnUrl}");

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByNameAsync(model.UserName);

        if (user == null)
        {
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            if(string.IsNullOrEmpty(model.ReturnUrl))
            {
                return Redirect("http://localhost:3000/");
            }
            return RedirectToLocal(model.ReturnUrl);
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Logout(string? logoutId)
    {
        // Get the logout context
        var logout = await _interaction.GetLogoutContextAsync(logoutId);

        // Sign out from ASP.NET Identity immediately without showing UI
        if (User?.Identity?.IsAuthenticated == true)
        {
            await _signInManager.SignOutAsync();
        }

        // Determine the post logout redirect URI
        var postLogoutRedirectUri = logout?.PostLogoutRedirectUri;

        // If no post logout redirect URI, default to SPA
        if (string.IsNullOrEmpty(postLogoutRedirectUri))
        {
            postLogoutRedirectUri = "http://localhost:3000/";
        }

        // Redirect immediately to the post logout redirect URI
        return Redirect(postLogoutRedirectUri);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout(LogoutModel model)
    {
        // Redirect to GET method for silent logout
        return await Logout(model.LogoutId);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        var user = await _userManager.FindByNameAsync(model.UserName);

        if (user != null)
        {
            return View("Success");
        }

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }

        await _eventBus.SendAsync(new UserCreatedEvent
        {
            UserId = user.Id,
            Email = user.Email
        });


        return View("Success");
    }

    public IActionResult Error()
    {
        return View();
    }
    public IActionResult AccessDenied()
    {
        return View();
    }

    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        else
        {
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
