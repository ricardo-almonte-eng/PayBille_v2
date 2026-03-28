# .NET Core API Design Patterns

## Dependency Injection
Always use constructor-based injection for testability.

## Error Handling
- Use `ActionResult<T>` for typed returns
- Throw domain exceptions from services
- Catch in middleware or controllers

## Validation
- Use DataAnnotations on DTOs
- Validate business rules in services
- Return 400 Bad Request for invalid input

## Async/Await
- Mark I/O operations as async
- Use `ConfigureAwait(false)` in libraries
- Always await async operations

## Logging
- Use ILogger<T> injected via DI
- Log at appropriate levels (Info, Warn, Error)
- Include context in log messages

## Repository Pattern
- Abstract data access behind interfaces
- Keep repositories focused on data queries
- Business logic stays in services

## Status Codes
- 200 OK: Successful GET/PUT request
- 201 Created: Successful POST with new resource
- 204 No Content: Successful DELETE
- 400 Bad Request: Invalid input
- 404 Not Found: Resource doesn't exist
- 409 Conflict: Business rule violation
- 500 Internal Server Error: Unhandled exception
