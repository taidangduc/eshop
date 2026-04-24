using EShop.IdentityService.ConfigurationOptions;
using EShop.IdentityService.Data;
using Microsoft.EntityFrameworkCore;
using EShop.Contracts.Customer.Services;
using EShop.IdentityService.Services;
using EShop.IdentityService.Entities;
using Microsoft.AspNetCore.Identity;
using EShop.IdentityService.Configurations;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.AddServiceDefaults();

var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);

// Configure PostgreSQL database context
builder.Services.AddDbContext<IdentityDbContext>(options =>
{
    var connectionString = appSettings.ConnectionStrings.EShopDb;
    options.UseNpgsql(connectionString);
    options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
});

// Configure AspNetCore Identity
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
// ref: https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
var IdSBuilder = builder.Services
.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
    options.EmitStaticAudienceClaim = true;
})
.AddInMemoryIdentityResources(IdSConfigurations.IdentityResources)
.AddInMemoryApiScopes(IdSConfigurations.ApiScopes)
.AddInMemoryApiResources(IdSConfigurations.ApiResources)
.AddInMemoryClients(IdSConfigurations.Clients)
.AddAspNetIdentity<User>();

if (builder.Environment.IsDevelopment()) IdSBuilder.AddDeveloperSigningCredential();

// Configure application cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
});

// Configure external authentication providers
var externalLogin = appSettings.ExternalLogin;
var authBuilder = builder.Services.AddAuthentication();

if (externalLogin?.Google?.IsEnabled == true)
{
    authBuilder.AddGoogle("Google", options =>
    {
        options.SignInScheme = IdentityConstants.ExternalScheme;
        options.ClientId = externalLogin.Google.ClientId;
        options.ClientSecret = externalLogin.Google.ClientSecret;
    });
}

if (externalLogin?.Facebook?.IsEnabled == true)
{
    authBuilder.AddFacebook("Facebook", options =>
    {
        options.SignInScheme = IdentityConstants.ExternalScheme;
        options.AppId = externalLogin.Facebook.AppId;
        options.AppSecret = externalLogin.Facebook.AppSecret;
    });
}
// Add scoped services for seeding data
builder.Services.AddScoped<IdentityDataContextSeed>();

// Add HttpClient for external API calls
builder.Services.AddHttpClient<ICustomerService, CustomerService>(client =>
{
    client.BaseAddress = new Uri(appSettings.Services.Customer.BaseUrl);
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Service default endpoints
app.MapDefaultEndpoints();

app.UseIdentityServer();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Migrate database and seed data on application startup
using (var scope = app.Services.CreateScope())
{
    var database = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
    await database.Database.MigrateAsync();

    var seeder = scope.ServiceProvider.GetRequiredService<IdentityDataContextSeed>();
    await seeder.SeedAsync();
}

app.Run();

public partial class Program { }
