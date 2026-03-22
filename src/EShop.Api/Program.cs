using EShop.Api;
using EShop.Api.Endpoints;
using EShop.Application;
using EShop.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddApplicationServices();
builder.AddInfrastructure();
builder.AddPersistence();

var app = builder.Build();

app.MapInfrastructure();
app.MapPersistence();

app.MapCatalogApi();
app.MapBasketApi();
app.MapOrderApi();
app.MapCustomerApi();
app.MapPaymentApi();

await app.MigrateAndSeedDataAsync();

app.Run();

public partial class Program { }