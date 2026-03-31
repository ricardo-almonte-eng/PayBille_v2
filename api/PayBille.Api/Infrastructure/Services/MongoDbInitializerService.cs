using MongoDB.Driver;
using PayBille.Api.Models;

namespace PayBille.Api.Infrastructure.Services;

/// <summary>
/// Service for initializing MongoDB collections, indexes, and seeding data.
/// </summary>
public sealed class MongoDbInitializerService
{
    private readonly MongoDbContext _context;
    private readonly ILogger<MongoDbInitializerService> _logger;

    public MongoDbInitializerService(MongoDbContext context, ILogger<MongoDbInitializerService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Initializes MongoDB collections and indexes.
    /// </summary>
    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await InitializePersonasCollectionAsync(cancellationToken);
            await InitializeEmpresasCollectionAsync(cancellationToken);
            await SeedUsuarioAdminAsync(cancellationToken);
            _logger.LogInformation("MongoDB initialization completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during MongoDB initialization.");
            throw;
        }
    }

    private async Task InitializePersonasCollectionAsync(CancellationToken cancellationToken = default)
    {
        var personasCollection = _context.Database.GetCollection<Persona>("personas");

        // Create unique index on idPersona (business identifier)
        var idPersonaIndexModel = new CreateIndexModel<Persona>(
            Builders<Persona>.IndexKeys.Ascending(p => p.IdPersona),
            new CreateIndexOptions { Unique = true, Name = "idx_persona_idPersona_unique" });

        try
        {
            await personasCollection.Indexes.CreateOneAsync(idPersonaIndexModel, null, cancellationToken);
            _logger.LogInformation("Created unique index on 'personas' collection for 'idPersona' field.");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on personas.idPersona");
        }

        // Create unique index on usuario.nombreUsuario
        var nombreUsuarioIndexModel = new CreateIndexModel<Persona>(
            Builders<Persona>.IndexKeys.Ascending(p => p.Usuario.NombreUsuario),
            new CreateIndexOptions { Unique = true, Name = "idx_persona_usuario_nombreUsuario_unique" });

        try
        {
            await personasCollection.Indexes.CreateOneAsync(nombreUsuarioIndexModel, null, cancellationToken);
            _logger.LogInformation("Created unique index on 'personas' collection for 'usuario.nombreUsuario' field.");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on personas.usuario.nombreUsuario");
        }

        // Create index on creadoEnUtc for sorting
        var creadoEnIndexModel = new CreateIndexModel<Persona>(
            Builders<Persona>.IndexKeys.Descending(p => p.CreadoEnUtc),
            new CreateIndexOptions { Name = "idx_persona_creadoEnUtc" });

        try
        {
            await personasCollection.Indexes.CreateOneAsync(creadoEnIndexModel, null, cancellationToken);
            _logger.LogInformation("Created index on 'personas' collection for 'creadoEnUtc' field.");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on personas.creadoEnUtc");
        }
    }

    private async Task InitializeEmpresasCollectionAsync(CancellationToken cancellationToken = default)
    {
        var empresasCollection = _context.Database.GetCollection<Empresa>("empresas");

        // Índice único en idEmpresa (identificador de negocio)
        var idEmpresaIndexModel = new CreateIndexModel<Empresa>(
            Builders<Empresa>.IndexKeys.Ascending(e => e.IdEmpresa),
            new CreateIndexOptions { Unique = true, Name = "idx_empresa_idEmpresa_unique" });

        try
        {
            await empresasCollection.Indexes.CreateOneAsync(idEmpresaIndexModel, null, cancellationToken);
            _logger.LogInformation("Created unique index on 'empresas' collection for 'idEmpresa' field.");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on empresas.idEmpresa");
        }

        // Índice en nombre para búsquedas
        var nombreIndexModel = new CreateIndexModel<Empresa>(
            Builders<Empresa>.IndexKeys.Ascending(e => e.Nombre),
            new CreateIndexOptions { Name = "idx_empresa_nombre" });

        try
        {
            await empresasCollection.Indexes.CreateOneAsync(nombreIndexModel, null, cancellationToken);
            _logger.LogInformation("Created index on 'empresas' collection for 'nombre' field.");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on empresas.nombre");
        }

        // Índice en activo para filtrar empresas activas
        var activoIndexModel = new CreateIndexModel<Empresa>(
            Builders<Empresa>.IndexKeys.Ascending(e => e.Activo),
            new CreateIndexOptions { Name = "idx_empresa_activo" });

        try
        {
            await empresasCollection.Indexes.CreateOneAsync(activoIndexModel, null, cancellationToken);
            _logger.LogInformation("Created index on 'empresas' collection for 'activo' field.");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on empresas.activo");
        }

        // Índice en creadoEnUtc para ordenamiento
        var creadoEnIndexModel = new CreateIndexModel<Empresa>(
            Builders<Empresa>.IndexKeys.Descending(e => e.CreadoEnUtc),
            new CreateIndexOptions { Name = "idx_empresa_creadoEnUtc" });

        try
        {
            await empresasCollection.Indexes.CreateOneAsync(creadoEnIndexModel, null, cancellationToken);
            _logger.LogInformation("Created index on 'empresas' collection for 'creadoEnUtc' field.");
        }
        catch (MongoCommandException ex) when (ex.Message.Contains("already exists"))
        {
            _logger.LogInformation("Index already exists on empresas.creadoEnUtc");
        }
    }

    /// <summary>
    /// Crea el usuario administrador por defecto si no existe ningún usuario en la colección.
    /// Solo se ejecuta una vez (primer arranque).
    /// </summary>
    private async Task SeedUsuarioAdminAsync(CancellationToken cancellationToken = default)
    {
        var personasCollection = _context.Database.GetCollection<Persona>("personas");

        var hayPersonas = await personasCollection
            .Find(Builders<Persona>.Filter.Empty)
            .Limit(1)
            .AnyAsync(cancellationToken);

        if (hayPersonas)
        {
            _logger.LogInformation("Colección 'personas' ya contiene datos. Se omite el seed del admin.");
            return;
        }

        const string contrasenaDefault = "Admin@123";
        var idPersona = Guid.NewGuid().ToString();

        var admin = new Persona
        {
            IdPersona    = idPersona,
            PrimerNombre = "Administrador",
            CreadoEnUtc  = DateTime.UtcNow,
            Usuario = new UsuarioPersona
            {
                NombreUsuario  = "admin",
                ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(contrasenaDefault),
                IdRol          = 1,
                Activo         = true,
                Master         = true
            }
        };

        await personasCollection.InsertOneAsync(admin, cancellationToken: cancellationToken);

        _logger.LogWarning(
            "════════════════════════════════════════════════════════════════\n" +
            "  USUARIO ADMINISTRADOR INICIAL CREADO\n" +
            "  Usuario:    admin\n" +
            "  Contraseña: {Contrasena}\n" +
            "  ⚠ Cambia esta contraseña después del primer login.\n" +
            "════════════════════════════════════════════════════════════════",
            contrasenaDefault);
    }
}
