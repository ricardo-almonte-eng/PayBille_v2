# PayBille v2 — Contexto del Proyecto

> Archivo de referencia para agentes. Describe las decisiones arquitectónicas, convenciones y patrones establecidos en el backend .NET.

---

## Stack Tecnológico

| Capa | Tecnología |
|---|---|
| Runtime | .NET 10 |
| Framework | ASP.NET Core Web API |
| Base de datos | MongoDB (driver oficial) |
| Autenticación | JWT Bearer + Refresh Tokens |
| Validación | FluentValidation.AspNetCore 11.3.1 |
| Hashing | BCrypt.Net |
| Testing API | Bruno (colección en `api/bruno/`) |

---

## Estructura del Proyecto

```
api/
├── PayBille.Api/
│   ├── Common/              # Result<T>
│   ├── Configuration/       # JwtSettings, MongoDbSettings
│   ├── Controllers/         # AuthController, PersonaController, HealthController
│   ├── DTOs/
│   │   ├── ApiRespDto.cs    # Wrapper genérico de respuesta
│   │   ├── Auth/            # UserLoginReqDto, UserRefreshTokenReqDto, UserAuthResDto
│   │   └── Persona/         # PersonaReqDto, PersonaResDto
│   ├── Errors/              # AppError, AppErrors (catálogo estático)
│   ├── Infrastructure/
│   │   ├── GlobalExceptionHandler.cs
│   │   ├── IMongoRepository.cs
│   │   ├── MongoDbContext.cs
│   │   ├── MongoRepository.cs
│   │   ├── Repositories/    # PersonaRepository
│   │   └── Services/        # MongoDbInitializerService
│   ├── Interfaces/          # IJwtService, IPersonaService, IHealthService
│   ├── Models/              # Persona, UsuarioPersona, RefreshToken
│   ├── Services/            # JwtService, PersonaService, HealthService
│   ├── Validators/          # PersonaReqDtoValidator, UserLoginReqDtoValidator
│   └── Program.cs
└── bruno/
    ├── bruno.json
    ├── environments/dev.bru
    ├── Auth/
    ├── Health/
    └── Persona/
```

---

## Convenciones Obligatorias

### Nombres de Entidades
- Todos los nombres de entidades, propiedades y métodos en **español**
- Ejemplo: `Persona`, `PrimerNombre`, `ObtenerTodosAsync`

### Repositorios
- Almacenar siempre `private readonly MongoDbContext _dbContext`
- La colección se accede mediante propiedad computada:
  ```csharp
  private IMongoCollection<T> Collection
      => _dbContext.Database.GetCollection<T>("nombre_coleccion");
  ```

### Identificadores de Documento
- Cada documento tiene un campo `Id{Entidad}` de tipo `string` (GUID)
- Se genera automáticamente en el servidor: `Guid.NewGuid().ToString()`
- Tiene índice único en MongoDB
- El cliente **nunca** envía este campo en el request (POST sin ID)
- Para actualizar: `PUT /api/{entidad}/{id}` con el GUID en la ruta

### Modelo MongoDB
```csharp
[BsonElement("idEntidad")]
[BsonRequired]
public string IdEntidad { get; set; } = string.Empty;
```
Índice en `MongoDbInitializerService`:
```csharp
new CreateIndexModel<T>(
    Builders<T>.IndexKeys.Ascending(x => x.IdEntidad),
    new CreateIndexOptions { Unique = true, Name = "idx_{coleccion}_id{Entidad}_unique" });
```

---

## Patrón de Respuesta HTTP

### Contrato de Status HTTP

| HTTP Code | Cuándo |
|---|---|
| `200` | **Siempre** — éxito y errores de negocio viajan en el body |
| `401` | JWT middleware challenge (token ausente/inválido) vía `OnChallenge` |
| `500` | Solo `GlobalExceptionHandler` — excepciones no controladas |

### ApiRespDto\<T\>
```csharp
// Éxito
{ "code": "0", "data": {...}, "message": null }

// Error de negocio
{ "code": "PEI01", "data": null, "message": "No encontramos a la persona..." }

// Error crítico (500)
{ "code": "APC01", "data": null, "message": "Ocurrió un error crítico..." }
```

---

## Patrón Result\<T\>

Ubicación: `Common/Result.cs`

```csharp
public sealed class Result<T>
{
    public bool      IsSuccess { get; }
    public T?        Value     { get; }
    public AppError? Error     { get; }

    public static Result<T> Ok(T value)          => new(value);
    public static Result<T> Fail(AppError error) => new(error);
}
```

**Regla:** Los servicios **nunca** lanzan excepciones para errores de dominio. Retornan `Result.Fail(AppErrors.XxxYyy())`.

**En controladores:**
```csharp
var result = await _service.ObtenerPorIdAsync(id, ct);
return Ok(result.IsSuccess
    ? ApiRespDto<PersonaResDto>.Ok(result.Value!)
    : ApiRespDto<PersonaResDto>.Error(result.Error!));
```

---

## Catálogo de Errores (AppErrors)

Ubicación: `Errors/AppErrors.cs` y `Errors/AppError.cs`

Esquema de códigos: `[Prefijo Controlador][Prefijo Acción][Seq]`

| Prefijo | Entidad/Acción |
|---|---|
| `PE` | Persona |
| `AU` | Auth |
| `AP` | Aplicación/Global |
| `G` | GetAll |
| `I` | GetById |
| `C` | Create/Update |
| `D` | Delete |
| `L` | Login |
| `R` | Refresh |
| `O` | Logout |
| `A` | Auth challenge |
| `C` | Critical (global) |

**Agregar un error nuevo:**
```csharp
// En AppErrors.cs
/// <summary>XY01 — Descripción del escenario.</summary>
public static AppError NombreDescriptivo()
    => AppError.From("XY01", "Mensaje en español para el usuario.");
```

---

## Catálogo de Errores Actuales

| Código | Método | Descripción |
|---|---|---|
| PEG01 | `PersonaListaErrorInterno` | Error interno al listar personas |
| PEI01 | `PersonaNoEncontrada(string id)` | Persona no encontrada |
| PEI02 | `PersonaBuscarErrorInterno` | Error interno al buscar |
| PEC01 | `PersonaValidacionFallida(string detalle)` | Validación fallida |
| PEC02 | `PersonaContrasenaRequerida` | Contraseña requerida al crear |
| PEC03 | `PersonaCrearErrorInterno` | Error interno al guardar |
| PED01 | `PersonaEliminarNoEncontrada(string id)` | Persona no encontrada al eliminar |
| PED02 | `PersonaEliminarErrorInterno` | Error interno al eliminar |
| AUL01 | `AuthCredencialesInvalidas` | Usuario o contraseña incorrectos |
| AUL02 | `AuthLoginErrorInterno` | Error interno al login |
| AUR01 | `AuthTokenInvalido` | Token inválido o expirado |
| AUR02 | `AuthRefreshErrorInterno` | Error interno al refrescar |
| AUO01 | `AuthLogoutErrorInterno` | Error interno al logout |
| AUA01 | `AuthSinToken` | JWT challenge 401 |
| APC01 | `ErrorCriticoInterno` | Excepción no controlada 500 |

---

## Patrón de Validación (FluentValidation)

- `SuppressModelStateInvalidFilter = true` — deshabilita el 400 automático
- Validación se llama manualmente en el controller:
  ```csharp
  var validation = await _validator.ValidateAsync(request, ct);
  if (!validation.IsValid)
  {
      var detalle = string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage));
      return Ok(ApiRespDto<T>.Error(AppErrors.XxxValidacionFallida(detalle)));
  }
  ```
- Registrar todos los validadores con: `AddValidatorsFromAssemblyContaining<TValidator>()`

---

## Estructura de Modelos MongoDB

### Persona (colección: `personas`)
```csharp
Persona {
    Id: string (ObjectId, BsonId)
    IdPersona: string (GUID, unique index)
    PrimerNombre: string (required)
    Apellido: string?
    Identificacion: string?
    TipoIdentificacion: string?
    IdMarket: int?
    Imagen: string?
    Usuario: UsuarioPersona (subdocumento)
    CreadoEnUtc: DateTime
}

UsuarioPersona {
    NombreUsuario: string (unique index)
    ContrasenaHash: string (BCrypt)
    Email: string?
    IdRol: int
    Activo: bool
    Torning: string?
    Master: bool?
    TokensRefresh: List<RefreshToken> (max 5, LIFO)
}
```

### Inicialización de Índices
Siempre crear/verificar índices en `MongoDbInitializerService.InitializeAsync()`.

---

## Endpoints Actuales

### Auth (`/api/auth`) — público
| Método | Ruta | Descripción |
|---|---|---|
| POST | `/login` | Login, retorna access + refresh token |
| POST | `/refresh` | Rota refresh token |
| POST | `/logout` | Revoca refresh token (requiere JWT) |

### Persona (`/api/persona`) — requiere JWT
| Método | Ruta | Descripción |
|---|---|---|
| GET | `/` | Lista todas las personas |
| GET | `/{id}` | Obtiene una persona por GUID |
| POST | `/` | Crea persona nueva (GUID generado en servidor) |
| PUT | `/{id}` | Actualiza persona existente |
| DELETE | `/{id}` | Elimina persona |

### Health (`/api/health`) — público
| Método | Ruta | Descripción |
|---|---|---|
| GET | `/` | Estado de salud del servicio |

---

## Bruno API Collection

Ubicación: `api/bruno/`

Variables de entorno (`dev`):
- `baseUrl` = `http://localhost:5000`
- `accessToken` — se llena automáticamente al ejecutar Login
- `refreshToken` — se llena automáticamente al ejecutar Login
- `personaId` — se llena automáticamente al ejecutar Crear

Los scripts `post-response` en Login y Crear propagan las variables automáticamente.

---

## Registro de Servicios en Program.cs

Al agregar una nueva entidad, registrar siempre en este orden:
```csharp
// 1. Repositorio específico
builder.Services.AddScoped<NuevaEntidadRepository>();

// 2. Servicio
builder.Services.AddScoped<INuevaEntidadService, NuevaEntidadService>();

// 3. Validador (auto-registrado si está en el mismo ensamblado)
// AddValidatorsFromAssemblyContaining<> ya lo cubre
```
