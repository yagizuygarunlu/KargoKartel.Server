# KargoKartel.Server

KargoKartel.Server is a modern, layered .NET 9 Web API application for cargo management and user authentication. It demonstrates clean architecture, CQRS, MediatR, and test-driven development best practices.

## Layers & Key Features

- **Domain:** Core entities, value objects, and business rules (e.g., `Cargo`, `Person`, `Status`, `AppUser`).
- **Application:**
  - CQRS (Command/Query) handlers (e.g., `CargoCreateCommand`, `CargoGetAllQuery`).
  - Validation with FluentValidation.
  - Handler orchestration via MediatR.
  - Authentication logic (`LoginCommand`, `RegisterCommand`).
- **Infrastructure:**
  - Data access with Entity Framework Core.
  - UserManager, SignInManager, JWT services.
  - Migrations and configuration files.
- **WebAPI:**
  - Minimal API endpoints.
  - JWT-based authentication.
  - Cargo CRUD and Auth endpoints.
  - Rate limiting, CORS, global exception handling.
- **Tests:**
  - Unit tests with xUnit, NSubstitute, and Shouldly.
  - Comprehensive coverage for all command and query handlers.

---

## Quick Start

```bash
# Restore dependencies
dotnet restore

# (Optional) Apply database migrations
dotnet ef database update --project KargoKartel.Server.Infrastructure

# Run the application
dotnet run --project WebAPI

# Run tests
dotnet test KargoKartel.Server.Application.Tests
```

---

## API Examples

- `POST /api/v1/register` : User registration
- `POST /api/v1/login` : JWT login
- `GET /api/v1/cargos` : List cargos (requires auth)
- `POST /api/v1/cargos` : Create new cargo
- `PUT /api/v1/cargos/{id}` : Update cargo
- `PUT /api/v1/cargos/{id}/status` : Update cargo status
- `DELETE /api/v1/cargos/{id}` : Delete cargo

---

## Contribution & License

- Contributions are welcome via pull requests.
- Open source under the [MIT License](LICENSE.txt).

---

## Showcase Notes

- Built with modern .NET 9, CQRS, MediatR, Minimal API, JWT, xUnit, NSubstitute, and Shouldly.
- Focused on code readability and testability.
