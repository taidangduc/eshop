# Eshop

> 🛍️ A practical e-commerce application built with .NET for the backend and React (Vite) for the frontend, demonstrating clean architecture and the latest technologies.
## Purpose

Provide a sample production-ready foundation for an online store with clean architecture and reliable messaging.

## Key Features

- HTTP APIs for Basket, Catalog, Customer, Identity, Order, and Payment.
- Domain‑driven design: entities, value objects, and domain events.
- Application layer with commands/handlers for business workflows.
- Outbox pattern (EF) + EventBus for reliable event delivery and eventual consistency.
- JWT authentication and data protection utilities.
- Docker Compose development environment and nginx reverse-proxy configuration.
- Unit and integration test projects.

## Goals of This Project

- ❇️ Using `Clean Architecture` for architecture level.
- ❇️ Using `CQRS` implementation with `MediatR` library.
- ❇️ Using `Fluent Validation` and a `Validation Pipeline Behaviour` on top of `MediatR`.
- ❇️ Using an EventBus abstraction with an In-Memory implementation for local development and testing. 
- ❇️ Using `Outbox Pattern` for ensuring no message is lost and there is at At Least One Delivery
- ❇️ Using `Unit Testing` for testing small units and mocking
- ❇️ Using `Docker` for containerization
- ❇️ Support multiple payment providers (VNPAY, PayPal, …) through a provider-based architecture.
- ❇️ Using `Nginx` for reserve proxy
- ❇️ Using `Aspire` for local development, fast test

## Technologies - Libraries

- ✔️ `.NET 9` – .NET Framework and .NET Core, including ASP\.NET and ASP\.NET Core.
- ✔️ `EF Core` – Modern object-database mapper for .NET. It supports LINQ queries, change tracking, updates, and schema migrations.
- ✔️ `ASP.NET Core OpenAPI` – API specification for automatic documentation and client generation.
- ✔️ `MediatR` – Implements the Mediator pattern to support CQRS and decoupled architectures.
- ✔️ `FluentValidation` – Fluent, strongly-typed validation library for clean input validation.
- ✔️ `AutoMapper` – Simplifies object-to-object mapping between DTOs and domain models.
- ✔️ `Swagger UI` – Interactive API documentation and testing interface.
- ✔️ `Nginx` – High-performance reverse proxy and load balancer.
- ✔️ `.NET Aspire` – Cloud-native tooling for orchestrating and observing distributed applications.
- ✔️ `React + Vite` – Fast, modern frontend stack for building responsive user interfaces.
- ✔️ `xUnit.net` – Unit testing framework with strong CI/CD integration.

## When to Use

1. E-commerce or transactional applications.
2. A practical approach to Domain-Driven Design with Clean Architecture.

## Structure of Project

- `eshop.sln` — solution root.
- `src/Api` — HTTP API project and endpoint.
- `src/Application` — application services, commands, and handlers.
- `src/Domain` — domain entities, value objects, domain events.
- `src/Infrastructure` — persistence, external service adapters and dependency injection wiring.
- `src/EventBus` and `src/EventBus.InMemory` — event bus abstractions and an in-memory provider.
- `src/Outbox.EF` — Outbox pattern implementation backed by EF Core.
- `src/EShop.StoreFront` — web client with React + Vite
- `infra/docker-compose` — Docker Compose configs for dev and production scenarios.
- `infra/nginx` — nginx configuration used by compose setups.
- `tests/UnitTests` and `tests/IntegrationTests` — test projects.

## Development Setup

##### 1. Prerequisites:

- Install .NET SDK.
- Install Docker and Docker Compose.

##### 2. Build locally:

```powershell
dotnet build
```

##### 3. Run API only:

```powershell
dotnet run --project src/Api/Api.csproj
```
note: run with aspire

```powershell
aspire run --project ./eshop.sln
```

##### 4. Run with Docker Compose:

```powershell
docker-compose -f infra/docker-compose/docker-compose.dev.yml up --build
```

##### 5. Run tests:

```powershell
dotnet test tests/UnitTests/UnitTests.csproj
dotnet test tests/IntegrationTests/IntegrationTests.csproj
```




