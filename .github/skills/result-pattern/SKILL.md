---
name: result-pattern
description: "Use the Result<T> pattern in PayBille v2. Use when implementing services that return domain errors without throwing exceptions, or when updating code that still uses try/catch for flow control."
---

# Result Pattern

> Patrón central del backend. **Todo servicio de dominio retorna `Result<T>` en lugar de lanzar excepciones.**

## Archivos Involucrados

| Archivo | Propósito |
|---|---|
| `Common/Result.cs` | Tipo `Result<T>` |
| `DTOs/ApiRespDto.cs` | Wrapper de respuesta HTTP |
| `Errors/AppError.cs` | Tipo de error inmutable |
| `Errors/AppErrors.cs` | Catálogo de errores con códigos fijos |

---

## Result\<T\> — Implementación

```csharp
// Common/Result.cs
public sealed class Result<T>
{
    public bool      IsSuccess { get; }
    public T?        Value     { get; }
    public AppError? Error     { get; }

    private Result(T value)        { IsSuccess = true;  Value = value; }
    private Result(AppError error) { IsSuccess = false; Error = error; }

    public static Result<T> Ok(T value)          => new(value);
    public static Result<T> Fail(AppError error) => new(error);
}
```

---

## Cómo Usar en Servicios

### ✅ Correcto
```csharp
public async Task<Result<PersonaResDto>> ObtenerPorIdAsync(string id, CancellationToken ct)
{
    var filter  = Builders<Persona>.Filter.Eq(p => p.IdPersona, id);
    var persona = await _personaRepository.FindOneAsync(filter, ct);

    return persona is null
        ? Result<PersonaResDto>.Fail(AppErrors.PersonaNoEncontrada(id))
        : Result<PersonaResDto>.Ok(MapToDto(persona));
}
```

### ❌ Incorrecto (no hacer)
```csharp
// NO lanzar excepciones para errores de dominio esperados
if (persona is null)
    throw new KeyNotFoundException("Persona no encontrada");

// NO lanzar ArgumentException para validaciones de negocio
if (string.IsNullOrEmpty(request.Contrasena))
    throw new ArgumentException("La contraseña es obligatoria");
```

---

## Cómo Usar en Controladores

```csharp
// Patrón estándar — sin try/catch
var result = await _service.ObtenerPorIdAsync(id, cancellationToken);
return Ok(result.IsSuccess
    ? ApiRespDto<PersonaResDto>.Ok(result.Value!)
    : ApiRespDto<PersonaResDto>.Error(result.Error!));
```

---

## Cuándo Sí se Usan Excepciones

Las excepciones son correctas únicamente para:

| Escenario | Quién las captura |
|---|---|
| Error de conexión MongoDB | `GlobalExceptionHandler` → HTTP 500 + APC01 |
| Error de configuración al arrancar | Proceso muere, logs de startup |
| Error completamente inesperado | `GlobalExceptionHandler` → HTTP 500 + APC01 |

**Regla:** Si el error es algo que el usuario puede causar o que el código puede anticipar → `Result.Fail`. Si es una falla del sistema → deja que suba a `GlobalExceptionHandler`.

---

## GlobalExceptionHandler

```csharp
// Infrastructure/GlobalExceptionHandler.cs
// Captura TODO lo que no fue manejado
// Retorna HTTP 500 + APC01
public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken ct)
{
    _logger.LogError(exception, "Excepción no controlada.");
    httpContext.Response.StatusCode  = 500;
    httpContext.Response.ContentType = "application/json";
    await httpContext.Response.WriteAsJsonAsync(
        ApiRespDto<object>.Error(AppErrors.ErrorCriticoInterno()));
    return true;
}
```

Registrado en `Program.cs`:
```csharp
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
// ...
app.UseExceptionHandler(); // debe ser el PRIMERO en el pipeline
```

---

## HTTP Status Contract

| HTTP | Cuándo |
|---|---|
| `200` | Siempre (éxito Y errores de negocio viajan en body) |
| `401` | JWT `OnChallenge` middleware — token ausente o inválido |
| `500` | Solo `GlobalExceptionHandler` |

## Referencias
- [Project Context](../../PROJECT_CONTEXT.md)
- [Error Catalog Skill](../error-catalog/SKILL.md)
