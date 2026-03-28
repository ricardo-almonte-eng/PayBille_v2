---
name: dotnet-api-design
description: "Design and implement .NET Core REST APIs for PayBille v2. Use when creating controllers, services, DTOs, or implementing business logic with clean architecture patterns and dependency injection."
---

# .NET Core API Design

## When to Use
- Building a new REST API endpoint
- Creating business logic services
- Designing data transfer objects (DTOs)
- Planning API response/request structures
- Implementing validation and error handling

## Procedure

### 1. Plan the API Contract
- Define the HTTP method (GET, POST, PUT, DELETE)
- Plan request/response structures
- Consider status codes and error scenarios

### 2. Create the DTO
Use the [DTO template](./assets/dto-template.cs) with proper validation attributes.

### 3. Implement the Service
Use the [service template](./assets/service-template.cs) for business logic encapsulation.

### 4. Create the Controller
Use the [controller template](./assets/controller-template.cs) for REST endpoints.

### 5. Add Error Handling
- Use ActionResult<T> for typed responses
- Handle exceptions in the service layer
- Return appropriate HTTP status codes

## Best Practices
- Separate concerns: DTOs for API contracts, Services for logic, Controllers for routing
- Use dependency injection for all service dependencies
- Validate input at the DTO level
- Return meaningful error messages
- Document API endpoints with XML comments

## References
See [.NET Core Patterns](./references/dotnet-core-patterns.md) for advanced patterns.
