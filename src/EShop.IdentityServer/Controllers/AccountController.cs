using EShop.IdentityService.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Duende.IdentityServer.Services;
using EShop.Contracts.Customer.Services;
using EShop.Contracts.Customer.DTOs;
using EShop.IdentityService.Data.Entities;

namespace EShop.IdentityService.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly ICustomerService _customerService;

    public AccountController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IIdentityServerInteractionService interaction,
        ICustomerService customerService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _interaction = interaction;
        _customerService = customerService;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string returnUrl = null)
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToLocal(returnUrl);
        }

        ViewData["ReturnUrl"] = returnUrl;

        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginModel model)
    {
        ViewData["ReturnUrl"] = model.ReturnUrl;

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
            if (string.IsNullOrEmpty(model.ReturnUrl))
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
        var logout = await _interaction.GetLogoutContextAsync(logoutId);

        if (User?.Identity?.IsAuthenticated == true)
        {
            await _signInManager.SignOutAsync();
        }

        var postLogoutRedirectUri = logout?.PostLogoutRedirectUri;

        if (string.IsNullOrEmpty(postLogoutRedirectUri))
        {
            postLogoutRedirectUri = "http://localhost:3000/";
        }

        return Redirect(postLogoutRedirectUri);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout(LogoutModel model)
    {
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
            return View();
        }

        user = new User
        {
            UserName = model.UserName,
            Email = model.Email
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }

        try
        {
            var customer = new CreateCustomerModel
            {
                UserId = user.Id,
                Email = user.Email,
            };

            await _customerService.CreateAsync(customer);
        }
        catch
        {
            await _userManager.DeleteAsync(user);
            return View("Error");
        }

        return View("Login");
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
