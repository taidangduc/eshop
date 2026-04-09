//ref: https://devblogs.microsoft.com/dotnet/new-aspire-app-with-react/
//ref: https://aspire.dev/fundamentals/networking-overview/#ports-and-proxies
//ref: https://learn.microsoft.com/en-us/dotnet/aspire/architecture/overview

var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.EShop_Api>("apiservice");

var identityService = builder.AddProject<Projects.EShop_IdentityService>("identityservice")
    .WaitFor(apiService);

// test / dev local frontend with react and vite
var reactVite = builder.AddViteApp("webfrontend", "../EShop.StoreFront")
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithEndpoint("http", e => e.Port = 3000) // fixed port for frontend
    .WithEnvironment("BROWSER", "none");

builder.AddProject<Projects.EShop_Bff>("bff")
    .WaitFor(identityService);

builder.Build().Run();
