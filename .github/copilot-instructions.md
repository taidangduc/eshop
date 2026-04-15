# Eshop Codebase Instructions

## Architecture Overview

This is a .NET 9 e-commerce application using **Clean Architecture** with CQRS pattern and Domain-Driven Design.

### Project Structure
| Project | Path | Purpose |
|---------|------|---------|
| **EShop.Domain** | `src/EShop.Domain` | Core entities, value objects, domain events. No external dependencies. |
| **EShop.Application** | `src/EShop.Application` | Commands/queries with MediatR handlers, DTOs, validators, behaviors |
| **EShop.Infrastructure** | `src/EShop.Infrastructure` | Payment, email, storage, background workers |
| **EShop.Persistence** | `src/EShop.Persistence` | EF Core `EShopDbContext`, entity configurations, interceptors, repositories |
| **EShop.WebAPI** | `src/EShop.WebAPI` | ASP.NET Controllers-based REST API (`/api/v1/{resource}`) |
| **EShop.Bff** | `src/EShop.Bff` | Backend-for-Frontend: YARP reverse proxy + OIDC cookie auth |
| **EShop.Identity** | `src/EShop.Identity` | Identity/authentication service |
| **EShop.Contracts** | `src/EShop.Contracts` | Shared DTOs and contracts |
| **EShop.Migrator** | `src/EShop.Migrator` | Database migration runner utility |
| **EShop.ServiceDefaults** | `src/EShop.ServiceDefaults` | Aspire shared configuration and extensions |
| **EShop.AppHost** | `src/EShop.AppHost` | .NET Aspire orchestration for local development |
| **EShop.StoreFront** | `src/EShop.StoreFront` | React + Vite frontend |

### Dependency Flow
Dependencies point inward: WebAPI → Application → Domain ← Infrastructure ← Persistence

## Key Patterns & Practices

### 1. CQRS with MediatR
All business logic flows through MediatR requests:
- **Commands**: `*Command.cs` + `*CommandHandler.cs` (e.g., `CreateOrderCommand`)
- **Queries**: `*Query.cs` + `*QueryHandler.cs` (e.g., `GetOrderByIdQuery`)
- **Validators**: `*CommandValidator.cs` using FluentValidation — **commands only, not queries**
- Place in feature folders: `EShop.Application/{Feature}/{Commands|Queries}/`

**Pipeline behaviors** (executed in order):
1. `ActivityBehavior` - Distributed tracing via OpenTelemetry
2. `LoggingBehavior` - Request/response logging
3. `ValidationBehavior` - FluentValidation (auto-discovered via reflection, commands only)

### 2. Domain-Driven Design
- **Entities** inherit from `Entity<TKey>` or `AuditableEntity<TKey>` (adds `CreatedAt`, `UpdatedAt`, `Version`)
- **Aggregate roots** implement `IAggregateRoot` marker interface
- **Domain Events** track state changes (e.g., `OrderCreatedDomainEvent`)
  - Raised by calling `entity.AddDomainEvent(event)` within entity methods
  - Automatically dispatched by `DispatchDomainEventInterceptor` after `SaveChanges`
  - Handled by `INotificationHandler<TDomainEvent>` in `EShop.Application/{Feature}/EventHandlers/`
- **Value Objects** for type safety (e.g., `Money`, `Address` in `EShop.Domain/ValueObject/`)
- Use factory methods like `Order.Create()`
- **Interceptors** in `EShop.Persistence/Interceptors/`:
  - `AuditableEntityInterceptor` — Auto-sets `CreatedAt`, `UpdatedAt`, increments `Version`
  - `DispatchDomainEventInterceptor` — Publishes domain events to MediatR after `SaveChanges`

### 3. Payment Gateway Factory Pattern
Payment providers implement `IPaymentGateway`:
```csharp
var gateway = _factory.Resolve(request.Provider);  // Factory resolves by PaymentProvider enum
var result = await gateway.CreatePaymentUrl(request);
```
- **Currently implemented**: `StripePaymentGateway`
- **Defined in enum but not implemented**: `Vnpay`
- Add new providers in `EShop.Infrastructure/Payment/`
- Update `PaymentGatewayFactory.Resolve()` switch statement

### 4. Observability
Commands/queries automatically tracked with:
- **Metrics**: `CommandHandlerMetrics`/`QueryHandlerMetrics` (counters, histograms)
- **Tracing**: `CommandHandlerActivity`/`QueryHandlerActivity` (OpenTelemetry spans)
- Tags include handler name/type discovered via reflection
- Registered in `ApplicationServiceExtensions.cs` as singletons
- Default Aspire observability includes distributed tracing, logs aggregation, and metrics dashboards

### 5. Background Workers
Located in `EShop.Infrastructure/HostServices/`:
- `GracePeriodWorker` — Handles order grace period logic
- `SendEmailWorker` — Processes email notifications

### 6. BFF (Backend-for-Frontend)
`EShop.Bff` acts as a secure gateway for the React frontend:
- YARP reverse proxy routes requests to WebAPI
- OIDC cookie-based authentication (redirects to Identity service)
- Automatically adds Bearer token to proxied requests

## Development Workflows

### Build & Run
```powershell
# Build entire solution
dotnet build

# Run WebAPI only
dotnet run --project src/EShop.WebAPI/EShop.WebAPI.csproj

# Run with Aspire (recommended — orchestrates all services)
aspire run --project ./eshop.sln

# Individual service infrastructure (PostgreSQL, RabbitMQ)
docker compose -f tools/postgresql/docker-compose.yml up -d
docker compose -f tools/rabbitmq/docker-compose.yml up -d
```

### Aspire Development
Aspire orchestrates the full stack locally:
- **AppHost** (`src/EShop.AppHost/Program.cs`) defines all service dependencies
- Services orchestrated: WebAPI, Identity, BFF, React frontend (Vite, port 3000)
- Auto-configured connections and service discovery
- Built-in dashboard for monitoring at `http://localhost:15888`

### Testing
```powershell
dotnet test tests/EShop.UnitTests/EShop.UnitTests.csproj
dotnet test tests/EShop.IntegrationTests/EShop.IntegrationTests.csproj
```
- **Unit tests**: xUnit + Moq for mocking
- **Integration tests**: `Aspire.Hosting.Testing` for full stack testing

### Database Migrations
Managed via `EShop.Migrator` project and `EShop.Persistence`:
- Single `EShopDbContext` (implements `IUnitOfWork`) for all tables
- Migrations run on startup via the migrator

To create new migrations:
```powershell
dotnet ef migrations add MigrationName -p src/EShop.Persistence
```

## Common Tasks

### Adding New Command/Query
1. Create in `EShop.Application/{Feature}/Commands/` or `EShop.Application/{Feature}/Queries/`
2. Define request: `public record MyCommand(...) : IRequest<MyResult>;`
3. Create handler: `public class MyCommandHandler : IRequestHandler<MyCommand, MyResult>`
4. (Optional) Add validator co-located with the command:
   ```csharp
   public class MyCommandValidator : AbstractValidator<MyCommand> { ... }
   ```
   - Validators **only for commands**, never for queries
   - Auto-discovered by FluentValidation via reflection
   - Executed in `ValidationBehavior` pipeline before handler

### Adding New Entity
1. Create in `EShop.Domain/Entities/`, inherit `Entity<TKey>` or `AuditableEntity<TKey>`
2. Implement `IAggregateRoot` if it's an aggregate root
3. Add `DbSet<Entity>` to `EShopDbContext` in `EShop.Persistence/`
4. Create configuration in `EShop.Persistence/DbConfigurations/` implementing `IEntityTypeConfiguration<T>`
5. Generate migration: `dotnet ef migrations add AddEntity -p src/EShop.Persistence`

### Adding a New API Controller
Add a controller to `src/EShop.WebAPI/Controllers/`:
```csharp
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class MyResourceController : ControllerBase
{
    private readonly ISender _sender;
    public MyResourceController(ISender sender) => _sender = sender;

    [HttpPost]
    [ProducesResponseType(typeof(MyResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] MyCommand cmd)
        => Ok(await _sender.Send(cmd));
}
```

### Domain Events
1. Define event record in `EShop.Domain/Events/` implementing `IDomainEvent`
2. Raise it inside the entity: `AddDomainEvent(new MyDomainEvent(...))`
3. Handle in `EShop.Application/{Feature}/EventHandlers/` with `INotificationHandler<MyDomainEvent>`

## Configuration

- **Development**: `src/EShop.WebAPI/appsettings.Development.json`
- Connection strings use Aspire format via `builder.AddNpgsqlDbContext<EShopDbContext>(...)`
- Payment config in `StripeConf` section (appsettings)
- `EShop.ServiceDefaults` provides shared Aspire configuration (telemetry, health checks)

## Important Conventions

- **Naming**: Commands end with `Command`, queries with `Query`, handlers with `Handler`, validators with `Validator`
- **Transactions**: Handled manually via `repository.UnitOfWork.SaveChangesAsync()` — no TxBehavior pipeline
- **Async all the way**: Use `async/await` consistently
- **Dependency Injection**: Register services via extension methods in `*Extensions.cs` or `*ServiceExtensions.cs`
- **Validation**: Use FluentValidation, not data annotations
- **Domain events**: For internal state changes only; no EventBus or Outbox pattern is used — domain events are dispatched in-process via MediatR
- **Feature folders**: Group by feature (Orders, Products, Customers, etc.) not by layer
- **Namespaces**: Match project name prefix — e.g., `EShop.Application.Orders.Commands`

## Frontend

React + Vite app in `src/EShop.StoreFront/` communicates with the backend via `EShop.Bff` (which proxies to `EShop.WebAPI`).
