# Work Order Management API

A .NET 9 backend system for managing incidents and work orders in buildings, built using Clean Architecture and Domain-Driven Design principles.

This project demonstrates:
- Domain-driven design with aggregates and business rules
- JWT authentication and authorization
- Environment-based infrastructure configuration
- Unit and integration testing

  ## Architecture

The project follows a Clean Architecture approach:

- Domain: business entities and rules
- Application: use cases and orchestration
- Infrastructure: persistence and external concerns
- API: controllers and HTTP layer

Key concepts:
- Aggregates (WorkOrder lifecycle)
- Repository pattern
- Unit of Work

  ## Features

- Incident management
- Work order lifecycle (assign, start, complete, cancel)
- Technician assignment
- JWT authentication (Admin / Technician roles)
- Protected endpoints

  ## Tech Stack

- .NET 9
- ASP.NET Core Web API
- Entity Framework Core (SQL Server)
- JWT Authentication
- xUnit + FluentAssertions
- Integration testing with WebApplicationFactory

  ## Tests

Run all tests:

dotnet test

Includes:
- Unit tests for domain rules
- Integration tests for API endpoints

  ## Why this project

I built this project to strengthen my backend skills and demonstrate:

- Designing systems beyond simple CRUD
- Applying domain-driven design concepts
- Building testable and maintainable architectures
