# ProcHub

ProcHub is an end-to-end procurement system for tracking purchase orders and suppliers. It uses a layered .NET 10 backend (ASP.NET Core, EF Core, PostgreSQL) with a Windows desktop client built in WPF using the MVVM pattern.

> **Status:** Work in progress. The purchase order, supplier, payment term and shipping term flows are implemented end to end across the API and the WPF client.
**To Finish:** Integration tests, Application users overview, implement creating purchases orders menu to dashboardview, add status update endpoint for PO's, add archive for closed orders. Move API into docker container.
**To be implemented after release:** WPF views for articles, articlegroups and country endpoints, backend finished.


## Features

- **Purchase orders**: Create and view Regular, Express and Sample orders. Each order takes its payment and shipping terms from the supplier, based on the order type.
- **Order Stasusses**: Track an order through 13 statuses, from `Drafted` to `OrderClosed`, including `OrderDiscrepancy` and `Cancelled`.
- **Dates, references and payments**: Record confirmation, production, shipping, arrival and delivery dates, plus confirmation, invoice and shipment references, and deposit and balance payments.
- **Dashboard**: Overview of all open purchase orders in the WPF client, appropiate fields editable to track order state.
- **Suppliers**: Manage supplier details, main, secondary and sample shipping and payment terms, lead times,country, responsible buyers, etc.
- **Terms**: payment terms and shipping terms, seeded with defaults on first startup.
- **Articles and article groups**: Available through the API only, might be added to frontend later.
- **Users and roles**: `User`, `SuperUser` and `Admin` roles. Appropriate endpoints authorized to only SuperUser, Admin or both.
- **Authentication**: bearer token sign-in through ASP.NET Core Identity. Accounts lock for 15 minutes after 5 failed sign-in attempts.

## Architecture

The solution uses domain-driven design. The domain holds the business rules, and the solution is split into layers with dependencies pointing inward toward the domain.

### Domain-driven design

- **Extensive domain model**: entities such as `PurchaseOrder`, `Supplier` and `PaymentTerm` contains domain methods, not just data. Setters are private, and each change goes through a method that checks its invariants. For example, a deposit percentage must be between 0 and 1 with at most two decimal places.
- **Aggregates and domain logic**: `Supplier` decides which terms apply to an order type through `GetTerms`, and `PurchaseOrder` takes its terms from the supplier when it is created.
- **Domain errors**: rule violations throw a `DomainException`, which keeps business rules separate from other layer exceptions.


| Project | Responsibility |
| --- | --- |
| `ProcHub.Domain` | Entities such as `PurchaseOrder`, `Supplier` and `PaymentTerm`, etc. Setters are private, and invalid changes throw a `DomainException`. |
| `ProcHub.Application` | Uses commands with FluentValidation and query handlers. Defines infrastructure abstractions such as `IProcHubDbContext` and `IUserIdentityService`. **Optimistic Concurrency** applied to handlers for endpoints that will be called frequently. Uses PSQL Xmin row to track version, version properties added to appropiate records and Domain models. |
| `ProcHub.Infrastructure` | EF Core with PostgreSQL, ASP.NET Core Identity, migrations and database seeders. |
| `ProcHub.Contracts` | Request and response DTOs and enums shared between the API and the WPF client. |
| `ProcHub.Api` | Minimal API endpoints grouped by feature, such as `/api/auth`, `/api/purchaseorders` and `/api/suppliers`. |
| `ProcHub.Wpf` | WPF desktop client using CommunityToolkit.Mvvm. It has a navigation service, a bearer-token HTTP handler and a token store. |

## Tech stack

- .NET 10 and C#
- ASP.NET Core minimal APIs
- Entity Framework Core 10 with PostgreSQL 17
- ASP.NET Core Identity with bearer tokens
- WPF with CommunityToolkit.Mvvm
- xUnit for tests, Testcontainers for PostgreSQL integration tests
- Docker Compose for the local database

## Getting started

### Prerequisites

- .NET 10 SDK
- Docker, for PostgreSQL and the integration tests
- Windows, to run the WPF client

### 1. Start the database

Copy the example environment file and set a password:

```bash
cp .env.example .env
```

Edit `.env` and set `POSTGRES_PASSWORD`, then start PostgreSQL:

```bash
docker compose up -d
```

The database listens on `127.0.0.1:5432`.

### 2. Configure the API

The API reads its connection string from `ConnectionStrings:ProcHubDb`. It also needs the initial administrator account, which the identity seeder creates on first run. Set both with user secrets:

```bash
dotnet user-secrets --project src/ProcHub.Api set "ConnectionStrings:ProcHubDb" "Host=localhost;Port=5432;Database=prochub;Username=prochub;Password=<your POSTGRES_PASSWORD>"
dotnet user-secrets --project src/ProcHub.Api set "InitialAdmin:Email" "admin@example.com"
dotnet user-secrets --project src/ProcHub.Api set "InitialAdmin:Password" "<a strong password>"
```

### 3. Run the API

```bash
dotnet run --project src/ProcHub.Api
```

The API listens on `http://localhost:5098`. On startup it applies EF Core migrations, seeds roles, payment terms and shipping terms, and creates the initial admin. In the `Development` environment it also loads development seed data.

### 4. Run the WPF client

```bash
dotnet run --project src/ProcHub.Wpf
```

The client points to `http://localhost:5098/` by default, set in `src/ProcHub.Wpf/appsettings.json`. Sign in with the initial admin account.

## Running tests

Domain unit tests need no external services:

```bash
dotnet test tests/ProcHub.Domain.Tests
```

The integration test project uses Testcontainers to start a disposable PostgreSQL container, so Docker must be running:

```bash
dotnet test tests/ProcHub.IntegrationTests
```

## Database migrations

Migrations live in `src/ProcHub.Infrastructure/Persistence/Migrations` and run automatically at API startup. To add one, install the local EF Core tool (pinned in `dotnet-tools.json`) and run:

```bash
dotnet tool restore
dotnet ef migrations add <MigrationName> --project src/ProcHub.Infrastructure/ProcHub.Infrastructure.csproj --startup-project src/ProcHub.Api/ProcHub.Api.csproj
```

## Project structure

```
ProcHub.slnx
compose.yaml              # PostgreSQL for local development
src/
  ProcHub.Api/            # Minimal API endpoints, startup and DI
  ProcHub.Application/    # Use cases, validators, authorization
  ProcHub.Contracts/      # DTOs and enums shared with the client
  ProcHub.Domain/         # Entities and domain rules
  ProcHub.Infrastructure/ # EF Core, Identity, migrations, seeders
  ProcHub.Wpf/            # WPF desktop client (MVVM)
tests/
  ProcHub.Domain.Tests/       # xUnit domain tests
  ProcHub.IntegrationTests/   # API tests with Testcontainers
```
