# MongoDB Connection Setup - PayBille v2

## Configuración Completada ✅

Se ha configurado completamente la conexión a MongoDB con localhost. Aquí está lo que se ha implementado:

### Estructura de Proyecto
```
Infrastructure/
├── IMongoRepository.cs      # Interfaz genérica para operaciones CRUD
├── MongoRepository.cs       # Implementación genérica del repositorio
├── MongoDbContext.cs        # Contexto de MongoDB (actualizado)
├── Repositories/
│   └── UserRepository.cs    # Repositorio específico para User
└── Services/
    └── MongoDbInitializerService.cs  # Inicialización de índices
```

### Características Implementadas

#### 1. **Repositorio Genérico** (`IMongoRepository<T>`)
- Interfaz genérica para todas las operaciones CRUD
- Métodos como: `InsertOneAsync`, `FindAsync`, `UpdateOneAsync`, `DeleteByIdAsync`, etc.
- Acceso directo a la colección para queries avanzadas

#### 2. **Contexto Mejorado** (`MongoDbContext`)
- Método `GetRepository<T>(collectionName)` para crear repositorios
- Inyección de dependencias simplificada

#### 3. **Repositorio de Usuarios** (`UserRepository`)
- Especializado para operaciones con la colección `users`
- Métodos personalizados: `FindByUsernameAsync`, `UsernameExistsAsync`
- Hereda todas las operaciones CRUD genéricas

#### 4. **Inicializador de MongoDB** (`MongoDbInitializerService`)
- Crea índices automáticamente al iniciar la aplicación
- Índice único en `username` (previene usuarios duplicados)
- Índice en `createdAtUtc` (optimiza búsquedas por fecha)

### Configuración en appsettings.json

**appsettings.json (Producción)**
```json
{
  "MongoDb": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "paybille_v2"
  },
  ...
}
```

**appsettings.Development.json (Desarrollo)**
```json
{
  "MongoDb": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "paybille_v2_dev"
  },
  ...
}
```

## ⚙️ Requisitos Previos

### 1. **MongoDB Local Instalado**
Descargar e instalar MongoDB Community Edition:
- **Windows**: https://www.mongodb.com/docs/manual/tutorial/install-mongodb-on-windows/
- **macOS**: https://www.mongodb.com/docs/manual/tutorial/install-mongodb-on-macos/
- **Linux**: https://www.mongodb.com/docs/manual/tutorial/install-mongodb-on-ubuntu/

### 2. **Verificar que MongoDB está corriendo**

**En Windows (CMD):**
```powershell
# Si instalaste MongoDB como servicio
Get-Service MongoDB

# O inicia MongoDB manualmente
mongod --dbpath "C:\data\db"
```

**En macOS/Linux:**
```bash
# Verificar si están corriendo
ps aux | grep mongod

# O inicia MongoDB
mongod
```

### 3. **Conectarse a MongoDB (opcional)**
```bash
# Usando MongoDB CLI
mongosh

# O usando Mongo Compass (GUI)
# Descargar: https://www.mongodb.com/products/compass
```

## 🚀 Cómo Usar

### 1. **Inyectar el Repositorio en Servicios**

```csharp
public sealed class MyService
{
    private readonly UserRepository _userRepository;

    public MyService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> GetUserAsync(string id, CancellationToken ct = default)
    {
        return await _userRepository.FindByIdAsync(id, ct);
    }
}
```

### 2. **Usar el Repositorio Genérico**

```csharp
public sealed class GenericService<T> where T : class
{
    private readonly IMongoRepository<T> _repository;

    public GenericService(IMongoRepository<T> repository)
    {
        _repository = repository;
    }

    public async Task<List<T>> GetAllAsync(CancellationToken ct = default)
    {
        return await _repository.GetAllAsync(ct);
    }
}
```

### 3. **Operaciones CRUD Comunes**

```csharp
// Create
var newUser = new User { Username = "john", PasswordHash = "hash..." };
await _userRepository.InsertOneAsync(newUser);

// Read - por ID
var user = await _userRepository.FindByIdAsync(userId);

// Read - por username
var user = await _userRepository.FindByUsernameAsync("john");

// Update
var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
var update = Builders<User>.Update.Set(u => u.Role, "admin");
await _userRepository.UpdateOneAsync(filter, update);

// Delete
await _userRepository.DeleteByIdAsync(userId);

// Count
var count = await _userRepository.CountAsync(filter);

// Exists
bool exists = await _userRepository.ExistsAsync(filter);
```

## 🔍 Verificar la Conexión

### 1. **Ejecutar la Aplicación**
```bash
cd api/PayBille.Api
dotnet run
```

### 2. **Observar los Logs**
Deberías ver en la consola:
```
info: PayBille.Api.Infrastructure.Services.MongoDbInitializerService[0]
      Created unique index on 'users' collection for 'username' field.
info: PayBille.Api.Infrastructure.Services.MongoDbInitializerService[0]
      Created index on 'users' collection for 'createdAtUtc' field.
info: PayBille.Api.Infrastructure.Services.MongoDbInitializerService[0]
      MongoDB initialization completed successfully.
```

### 3. **Verificar en MongoDB**
```bash
mongosh

> use paybille_v2_dev
switched to db paybille_v2_dev

> db.users.getIndexes()
[ 
  { v: 2, key: { _id: 1 }, name: '_id_' },
  { v: 2, key: { username: 1 }, name: 'idx_username_unique', unique: true },
  { v: 2, key: { createdAtUtc: -1 }, name: 'idx_createdAtUtc' }
]
```

## 📝 Cambios Realizados

### Archivos Creados
- `Infrastructure/IMongoRepository.cs` - Interfaz genérica
- `Infrastructure/MongoRepository.cs` - Implementación genérica
- `Infrastructure/Repositories/UserRepository.cs` - Repositorio de usuarios
- `Infrastructure/Services/MongoDbInitializerService.cs` - Inicializador de índices

### Archivos Modificados
- `Program.cs` - Registró DI y ejecutó inicialización
- `MongoDbContext.cs` - Agregó método `GetRepository<T>()`
- `Services/JwtService.cs` - Ahora usa `UserRepository` en lugar de acceso directo

## 🔐 Seguridad

### Próximos pasos (cuando necesites)
1. **Autenticación en MongoDB**: Configurar usuario/contraseña
   ```json
   "ConnectionString": "mongodb://user:password@localhost:27017"
   ```

2. **Replica Sets**: Para transacciones ACID multi-documento
   ```json
   "ConnectionString": "mongodb://localhost:27017,localhost:27018,localhost:27019/?replicaSet=rs0"
   ```

3. **Encriptación**: Usar TLS/SSL
   ```json
   "ConnectionString": "mongodb://localhost:27017/?ssl=true&tlsCertificateKeyFile=path/to/cert.pem"
   ```

## 📚 Referencias
- [MongoDB .NET Driver Docs](https://www.mongodb.com/docs/drivers/csharp/)
- [MongoDB Query Language](https://www.mongodb.com/docs/manual/reference/)
- [Best Practices](https://www.mongodb.com/docs/manual/core/databases-and-collections/)

---

✅ **¡MongoDB está listo para usar!**
