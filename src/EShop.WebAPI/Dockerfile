FROM mcr.microsoft.com/dotnet/sdk:9.0 AS builder
WORKDIR /src

# copy csproj files (use repo root context: docker-compose sets context to project root)
COPY ["src/EShop.WebAPI/EShop.WebAPI.csproj", "EShop.WebAPI/"]
COPY ["src/EShop.Application/EShop.Application.csproj", "EShop.Application/"]
COPY ["src/EShop.Infrastructure/EShop.Infrastructure.csproj", "EShop.Infrastructure/"]
COPY ["src/EShop.ServiceDefaults/EShop.ServiceDefaults.csproj", "EShop.ServiceDefaults/"]
COPY ["src/EShop.Shared/EShop.Shared.csproj", "EShop.Shared/"]
COPY ["src/EShop.Domain/EShop.Domain.csproj", "EShop.Domain/"]
COPY ["src/EShop.Contracts/EShop.Contracts.csproj", "EShop.Contracts/"]
COPY ["src/EShop.EventBus/EShop.EventBus.csproj", "EShop.EventBus/"]
COPY ["src/EShop.EventBus.InMemory/EShop.EventBus.InMemory.csproj", "EShop.EventBus.InMemory/"]
COPY ["src/EShop.Outbox/EShop.Outbox.csproj", "EShop.Outbox/"]
COPY ["src/EShop.Outbox.EF/EShop.Outbox.EF.csproj", "EShop.Outbox.EF/"]

RUN dotnet restore "EShop.WebAPI/EShop.WebAPI.csproj"

# copy everything else and build
COPY src/ .
RUN dotnet build "EShop.WebAPI/EShop.WebAPI.csproj" -c Release --no-restore

# publish
RUN dotnet publish "EShop.WebAPI/EShop.WebAPI.csproj" -c Release --no-restore -o /app/publish

# final runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=builder /app/publish .

ENV ASPNETCORE_URLS http://+:80
ENV ASPNETCORE_ENVIRONMENT Docker

EXPOSE 80

ENTRYPOINT ["dotnet", "EShop.WebAPI.dll"]
