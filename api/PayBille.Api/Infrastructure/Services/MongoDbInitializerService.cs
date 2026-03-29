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
}
