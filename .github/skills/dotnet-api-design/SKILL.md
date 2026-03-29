---
name: dotnet-api-design
description: "Design and implement .NET Core REST APIs for PayBille v2. Use when creating controllers, services, DTOs, or implementing business logic with clean architecture patterns and dependency injection."
---

# .NET Core API Design

> **Siempre leer** [`PROJECT_CONTEXT.md`](../../PROJECT_CONTEXT.md) antes de implementar para respetar las convenciones establecidas.

## When to Use
- Building a new REST API endpoint
- Creating business logic services
- Designing data transfer objects (DTOs)
- Planning API response/request structures
- Implementing validation and error handling

## Procedure

### 1. Plan the API Contract
- Define the HTTP method (GET, POST, PUT, DELETE)
- **Todos los endpoints retornan HTTP 200** — los errores de negocio viajan en el body
- Solo excepción: `401` (JWT challenge), `500` (GlobalExceptionHandler)
- Plan request/response using `ApiRespDto<T>` wrapper

### 2. Define Errors First
Before implementing, add all foreseeable error cases to `Errors/AppErrors.cs`.
See [error-catalog skill](../error-catalog/SKILL.md) for naming rules.

### 3. Create the DTOs
- **ReqDto**: campos que el cliente envía. **Nunca incluir** el ID de entidad en POST (se genera en servidor).
- **ResDto**: campos que el servidor retorna.
- Use `[Required]` + FluentValidation validator.
- Use the [DTO template](./assets/dto-template.cs).

### 4. Implement the Interface
```csharp
Task<Result<List<EntidadResDto>>> ObtenerTodosAsync(CancellationToken ct);
Task<Result<EntidadResDto>> ObtenerPorIdAsync(string id, CancellationToken ct);
Task<Result<EntidadResDto>> CrearAsync(EntidadReqDto request, CancellationToken ct);
Task<Result<EntidadResDto>> ActualizarAsync(string id, EntidadReqDto request, CancellationToken ct);
Task<Result<bool>> EliminarAsync(string id, CancellationToken ct);
```

### 5. Implement the Service
- Return `Result<T>.Ok(valor)` on success
- Return `Result<T>.Fail(AppErrors.XxxYyy())` on domain errors
- **Never throw exceptions for domain errors**
- Use the [service template](./assets/service-template.cs).

### 6. Create the Controller
- Inject `IValidator<ReqDto>` for FluentValidation
- Call `_validator.ValidateAsync()` manually before service call
- Map result to `ApiRespDto<T>.Ok()` / `ApiRespDto<T>.Error()`
- Use the [controller template](./assets/controller-template.cs).

### 7. Register in Program.cs
```csharp
builder.Services.AddScoped<NuevaEntidadRepository>();
builder.Services.AddScoped<INuevaEntidadService, NuevaEntidadService>();
```

### 8. Add Bruno Requests
Add `.bru` files to `api/bruno/{Entidad}/` following the existing Persona structure.

## Checklist per Entity
- [ ] `Models/{Entidad}.cs` — con `Id{Entidad}: string`, `[BsonRequired]`, `[BsonElement]`
- [ ] `Infrastructure/Repositories/{Entidad}Repository.cs` — usa `_dbContext` convention
- [ ] Índice único en `MongoDbInitializerService`
- [ ] `DTOs/{Entidad}/{Entidad}ReqDto.cs`
- [ ] `DTOs/{Entidad}/{Entidad}ResDto.cs`
- [ ] `Interfaces/I{Entidad}Service.cs` — retornos `Result<T>`
- [ ] `Services/{Entidad}Service.cs` — sin excepciones de dominio
- [ ] `Validators/{Entidad}ReqDtoValidator.cs`
- [ ] `Controllers/{Entidad}Controller.cs` — sin try/catch
- [ ] `Errors/AppErrors.cs` — nuevos códigos para la entidad
- [ ] `Program.cs` — registro DI
- [ ] `api/bruno/{Entidad}/` — requests Bruno

## Best Practices
- Nombres de entidades, propiedades y métodos en **español**
- `_dbContext` en repositorios (no `_collection`)
- `Id{Entidad}` es GUID generado en servidor — el cliente nunca lo envía en POST
- Controladores sin `try/catch` — usan `result.IsSuccess`
- FluentValidation: `SuppressModelStateInvalidFilter = true`

## References
- [Project Context](../../PROJECT_CONTEXT.md)
- [Result Pattern](../result-pattern/SKILL.md)
- [Error Catalog](../error-catalog/SKILL.md)
- [.NET Core Patterns](./references/dotnet-core-patterns.md)
