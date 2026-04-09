using EShop.Api;
using EShop.Api.ConfigurationOptions;
using EShop.Application;
using EShop.Infrastructure.ExternalServices.Payment;
using EShop.Infrastructure.Notification;
using EShop.Infrastructure.Storage;
using EShop.Persistence;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

var appSettings = new AppSettings();
configuration.Bind(appSettings);

services.Configure<AppSettings>(configuration);

builder.AddServiceDefaults();

services
.AddHosting(appSettings)
.AddApplicationServices()
.AddPersistence(appSettings.ConnectionStrings.EShopDb)
.AddStorage(appSettings.Storage)
.AddPayment(appSettings.Payment)
.AddNotification(appSettings.Notification);

var app = builder.Build();

app.MapHosting();

await app.MigratePersistenceAsync();

app.Run();

public partial class Program { }
