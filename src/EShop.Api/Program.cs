using EShop.Api;
using EShop.Api.ConfigurationOptions;
using EShop.Api.Endpoints;
using EShop.Application;
using EShop.Infrastructure;
using EShop.Infrastructure.Storage;
using EShop.Persistence;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

var appSettings = new AppSettings();
configuration.Bind(appSettings);

services.Configure<AppSettings>(configuration);

builder.AddServiceDefaults();

builder
.AddApplicationServices()
.AddInfrastructure()
.AddHosting()
.AddPersistence(appSettings.ConnectionStrings.EShopDb);

services.AddStorage(appSettings.Storage);

var app = builder.Build();

app.MapInfrastructure();
app.MapHosting();

app.MapCatalogApi();
app.MapBasketApi();
app.MapOrderApi();
app.MapCustomerApi();
app.MapPaymentApi();

await app.MigrateAndSeedDataAsync();

app.Run();

public partial class Program { }