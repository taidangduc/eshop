using EShop.IdentityService.ConfigurationOptions;
using EShop.IdentityService.Extensions;
using EShop.IdentityService.Persistence;
using EShop.Migrator;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);

builder.AddApplicationServices(appSettings);

builder.Services.AddControllersWithViews();

var app = builder.Build();

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

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

await app.MigrationDbContextAsync<IdentityDbContext>();

app.Run();

public partial class Program { }
