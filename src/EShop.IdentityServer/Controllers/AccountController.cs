using EShop.IdentityService.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Duende.IdentityServer.Services;
using EShop.Contracts.Customer.Services;
using EShop.Contracts.Customer.DTOs;
using EShop.IdentityService.Entities;
using EShop.IdentityService.Authorization;

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
        if (User.Identity.IsAuthenticated == true)
        {
            return RedirectToLocal(returnUrl);
        }

        ViewData["ReturnUrl"] = returnUrl;

        return View();
    }

    // Flow: FE -> BFF -> IdentityServer -> BFF -> FE
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginModel model)
    {
        ViewData["ReturnUrl"] = model.ReturnUrl;

        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Invalid username or password.";
            return RedirectToAction("Login", new { returnUrl = model.ReturnUrl });
        }

        var user = await _userManager.FindByNameAsync(model.UserName);

        if (user == null)
        {
            TempData["Error"] = "Invalid username or password.";
            return RedirectToAction("Login", new { returnUrl = model.ReturnUrl });
        }

        var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            if (string.IsNullOrEmpty(model.ReturnUrl))
            {
                // If returnUrl is not provided, redirect to home page
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            return RedirectToLocal(model.ReturnUrl);
        }
        TempData["Error"] = "Invalid username or password.";
        return RedirectToAction("Login", new { returnUrl = model.ReturnUrl });
    }
    
    // Flow:
    // # Logout at IdenityServer (directly): 
    // User 
    // -> IdentityServer 
    // -> Logout 
    // -> Confirm Logout 
    // -> IdentityServer clears SSO session
    // -> Redirect IdentityServer Home Page
    // # Logout via BFF (silent logut):
    // User 
    // -> FE 
    // -> BFF clear local session 
    // -> IdentityServer logout 
    // -> Redirect Logout UI (silent) 
    // -> IdentityServer clears SSO session 
    // -> Redirect PostLogoutRedirectUri to BFF 
    // -> BFF redirects to FE Home Page
    [HttpGet]
    public async Task<IActionResult> Logout(string? logoutId)
    {
        var context = await _interaction.GetLogoutContextAsync(logoutId);

        var model = new LogoutModel
        {
            LogoutId = logoutId,
            PostLogoutRedirectUri = context?.PostLogoutRedirectUri,
            ShowLogoutPrompt = context?.ShowSignoutPrompt ?? User.Identity?.IsAuthenticated == true,
        };
 
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout(LogoutModel model)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            await _signInManager.SignOutAsync();
        }

        var context = await _interaction.GetLogoutContextAsync(model.LogoutId);
        var returnUrl = context?.PostLogoutRedirectUri;

        if (string.IsNullOrEmpty(returnUrl))
        {
            returnUrl = Url.Action("Index", "Home");
        }

        return Redirect(returnUrl);
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
            TempData["Error"] = "Invalid input.";
            return RedirectToAction("Register");
        }

        var user = await _userManager.FindByNameAsync(model.UserName);

        if (user != null)
        {
            TempData["Error"] = "Invalid input.";
            return RedirectToAction("Register");
        }

        user = new User
        {
            UserName = model.UserName,
            Email = model.Email
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            //foreach (var error in result.Errors)
            //{
            //    ModelState.AddModelError(string.Empty, error.Description);
            //}
            TempData["Error"] = "Invalid input.";
            return RedirectToAction("Register");
        }

        // Assign default role
        await _userManager.AddToRoleAsync(user, Roles.User);

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
            TempData["Error"] = "Occur error when create account !";
            return RedirectToAction("Register");
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
