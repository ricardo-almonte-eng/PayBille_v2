---
name: error-catalog
description: "Add or look up error codes in PayBille v2 AppErrors catalog. Use when creating new endpoints, adding error cases, or understanding what an error code means."
---

# Error Catalog

> Todos los errores controlados de dominio tienen un **código fijo** de 5 caracteres. Nunca se generan aleatoriamente.

## Archivos

| Archivo | Propósito |
|---|---|
| `Errors/AppError.cs` | Tipo inmutable: `Code` + `Message` |
| `Errors/AppErrors.cs` | Catálogo estático — un método por escenario |

---

## AppError — Implementación

```csharp
// Errors/AppError.cs
public sealed class AppError
{
    public string Code    { get; }
    public string Message { get; }

    private AppError(string code, string message) { Code = code; Message = message; }

    internal static AppError From(string code, string message) => new(code, message);
}
```

---

## Esquema de Códigos

```
[Prefijo Entidad][Prefijo Acción][Número secuencial]

Prefijos de Entidad:
  PE = Persona
  AU = Auth
  AP = Aplicación/Global
  (siguiente entidad: ej. FA = Factura, PR = Producto)

Prefijos de Acción:
  G = GetAll (ObtenerTodos)
  I = GetById (ObtenerPorId)
  C = Create/Update (Crear/Actualizar)
  D = Delete (Eliminar)
  L = Login
  R = Refresh
  O = Logout
  A = Auth challenge (middleware)
  C = Critical/Global (solo AP)

Ejemplo: PEI01 = Persona + GetById + error #1
```

---

## Catálogo Actual

### Persona

| Código | Método | Cuándo se usa |
|---|---|---|
| PEG01 | `PersonaListaErrorInterno()` | Error interno al listar |
| PEI01 | `PersonaNoEncontrada(string id)` | GET por ID no existe |
| PEI02 | `PersonaBuscarErrorInterno()` | Error interno en búsqueda |
| PEC01 | `PersonaValidacionFallida(string detalle)` | FluentValidation falló |
| PEC02 | `PersonaContrasenaRequerida()` | POST sin contraseña |
| PEC03 | `PersonaCrearErrorInterno()` | Error interno al guardar |
| PED01 | `PersonaEliminarNoEncontrada(string id)` | DELETE — no existe |
| PED02 | `PersonaEliminarErrorInterno()` | Error interno al eliminar |

### Auth

| Código | Método | Cuándo se usa |
|---|---|---|
| AUL01 | `AuthCredencialesInvalidas()` | Login fallido |
| AUL02 | `AuthLoginErrorInterno()` | Error interno en login |
| AUR01 | `AuthTokenInvalido()` | Refresh token inválido/expirado |
| AUR02 | `AuthRefreshErrorInterno()` | Error interno en refresh |
| AUO01 | `AuthLogoutErrorInterno()` | Error interno en logout |
| AUA01 | `AuthSinToken()` | JWT middleware 401 challenge |

### Global

| Código | Método | Cuándo se usa |
|---|---|---|
| APC01 | `ErrorCriticoInterno()` | GlobalExceptionHandler — HTTP 500 |

---

## Cómo Agregar Errores para una Nueva Entidad

### Paso 1 — Definir los códigos antes de implementar

Para la entidad `Factura` con prefijo `FA`:

| Código | Escenario |
|---|---|
| FAG01 | Error interno al listar facturas |
| FAI01 | Factura no encontrada |
| FAI02 | Error interno al buscar |
| FAC01 | Validación fallida |
| FAC02 | Error de negocio específico al crear |
| FAC03 | Error interno al crear |
| FAD01 | Factura no encontrada al eliminar |
| FAD02 | Error interno al eliminar |

### Paso 2 — Agregar al catálogo

```csharp
// En Errors/AppErrors.cs, agregar una sección nueva:

// ── Factura · ObtenerTodos ───────────────────────────────────────────────
/// <summary>FAG01 — Error interno al listar facturas.</summary>
public static AppError FacturaListaErrorInterno()
    => AppError.From("FAG01", "Ocurrió un error al obtener la lista de facturas.");

// ── Factura · ObtenerPorId ───────────────────────────────────────────────
/// <summary>FAI01 — No se encontró la factura con el identificador especificado.</summary>
public static AppError FacturaNoEncontrada(string id)
    => AppError.From("FAI01", $"No encontramos la factura con identificador {id}.");

// ... etc.
```

### Paso 3 — Usar en el servicio

```csharp
return factura is null
    ? Result<FacturaResDto>.Fail(AppErrors.FacturaNoEncontrada(id))
    : Result<FacturaResDto>.Ok(MapToDto(factura));
```

---

## Reglas

- **Un código por escenario** — no reusar el mismo código en distintos catch blocks
- **Mensajes en español** orientados al usuario final
- **Códigos únicos en todo el catálogo** — revisar antes de agregar
- Los métodos con parámetro `string id` lo incluyen en el mensaje para facilitar debugging
- `AppError.From` es `internal` — solo se llama desde `AppErrors`

## Referencias
- [Project Context](../../PROJECT_CONTEXT.md)
- [Result Pattern Skill](../result-pattern/SKILL.md)
