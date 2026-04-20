using Duende.IdentityServer;
using EShop.IdentityService.ConfigurationOptions;
using EShop.IdentityService.ConfigurationOptions.ExternalLogin;
using EShop.IdentityService.Configurations;
using EShop.IdentityService.Data.Entities;
using Microsoft.AspNetCore.Identity;
using EShop.IdentityService.Data;
using EShop.Migrator;
using EShop.IdentityService.Data.Seed;
using EShop.Contracts.Customer.Services;
using EShop.IdentityService.Services;

namespace EShop.IdentityService.Extensions;

public static class Extensions
{
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder, AppSettings appSettings)
    {
        builder.AddNpgsqlDbContext<IdentityDbContext>("EShopDb");

        // Configure Identity
        builder.Services.AddIdentity<User, Role>(config =>
        {
            config.Password.RequiredLength = 6;
            config.Password.RequireDigit = false;
            config.Password.RequireNonAlphanumeric = false;
            config.Password.RequireUppercase = false;
        })
        .AddEntityFrameworkStores<IdentityDbContext>()
        .AddDefaultTokenProviders();

        // Configure IdentityServer
        var identityServerBuilder = builder.Services
        .AddIdentityServer(options =>
        {
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;
            // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
            options.EmitStaticAudienceClaim = true;
        })
        .AddInMemoryIdentityResources(DuendeConfigurations.IdentityResources)
        .AddInMemoryApiScopes(DuendeConfigurations.ApiScopes)
        .AddInMemoryApiResources(DuendeConfigurations.ApiResources)
        .AddInMemoryClients(DuendeConfigurations.Clients)
        .AddAspNetIdentity<User>();

        if (builder.Environment.IsDevelopment()) identityServerBuilder.AddDeveloperSigningCredential();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Logout";
        });

        // Configure AppSettings
        var externalLogin = builder.Configuration.GetSection("ExternalLogin").Get<ExternalLoginOptions>();

        // Add external authentication providers
        var authenticationBuilder = builder.Services.AddAuthentication();

        if (externalLogin?.Google?.IsEnabled == true)
        {
            authenticationBuilder.AddGoogle("Google", options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                options.ClientId = externalLogin.Google.ClientId;
                options.ClientSecret = externalLogin.Google.ClientSecret;
            });
        }

        if (externalLogin?.Facebook?.IsEnabled == true)
        {
            authenticationBuilder.AddFacebook("Facebook", options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                options.AppId = externalLogin.Facebook.AppId;
                options.AppSecret = externalLogin.Facebook.AppSecret;
            });
        }

        builder.Services.AddScoped<IDataSeeder<IdentityDbContext>, IdentityDataSeeder>();

        builder.Services.AddHttpClient<ICustomerService, CustomerService>(client =>
        {
            client.BaseAddress = new Uri(appSettings.Services.Customer.BaseUrl);
        });

        return builder;
    }
}