//ref: https://devblogs.microsoft.com/dotnet/new-aspire-app-with-react/
//ref: https://aspire.dev/fundamentals/networking-overview/#ports-and-proxies
//ref: https://learn.microsoft.com/en-us/dotnet/aspire/architecture/overview

var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.EShop_WebAPI>("webapi");

var identityService = builder.AddProject<Projects.EShop_IdentityServer>("identityserver")
    .WaitFor(apiService);

// test / dev local frontend with react and vite
var reactVite = builder.AddViteApp("storefront", "../EShop.StoreFront")
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithEndpoint("http", e => e.Port = 3000) // fixed port for frontend
    .WithEnvironment("BROWSER", "none");

builder.AddProject<Projects.EShop_Bff>("bff")
    .WaitFor(identityService);

builder.Build().Run();
