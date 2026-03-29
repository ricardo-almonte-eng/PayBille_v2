using MongoDB.Driver;
using PayBille.Api.Models;

namespace PayBille.Api.Infrastructure.Repositories;

/// <summary>
/// Specialized repository for Persona document operations.
/// </summary>
public sealed class PersonaRepository : IMongoRepository<Persona>
{
    private readonly MongoDbContext _dbContext;

    public IMongoCollection<Persona> Collection => _dbContext.Database.GetCollection<Persona>("personas");

    public PersonaRepository(MongoDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        _dbContext = context;
    }

    /// <summary>
    /// Finds a persona by nombre de usuario.
    /// </summary>
    public async Task<Persona?> FindByNombreUsuarioAsync(string nombreUsuario, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Persona>.Filter.Eq(p => p.Usuario.NombreUsuario, nombreUsuario);
        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Checks if a nombre de usuario exists.
    /// </summary>
    public async Task<bool> NombreUsuarioExisteAsync(string nombreUsuario, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Persona>.Filter.Eq(p => p.Usuario.NombreUsuario, nombreUsuario);
        var options = new CountOptions { Limit = 1 };
        var count = await Collection.CountDocumentsAsync(filter, options, cancellationToken);
        return count > 0;
    }

    /// <summary>
    /// Inserts or replaces a persona document matched by IdPersona (upsert).
    /// </summary>
    public async Task UpsertAsync(Persona persona, CancellationToken cancellationToken = default)
    {
        var filter  = Builders<Persona>.Filter.Eq(p => p.IdPersona, persona.IdPersona);
        var options = new ReplaceOptions { IsUpsert = true };
        await Collection.ReplaceOneAsync(filter, persona, options, cancellationToken);
    }

    // Implement IMongoRepository<Persona> methods
    public async Task InsertOneAsync(Persona document, CancellationToken cancellationToken = default)
    {
        await Collection.InsertOneAsync(document, cancellationToken: cancellationToken);
    }

    public async Task InsertManyAsync(IEnumerable<Persona> documents, CancellationToken cancellationToken = default)
    {
        await Collection.InsertManyAsync(documents, cancellationToken: cancellationToken);
    }

    public async Task<Persona?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Persona>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(id));
        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Persona?> FindOneAsync(FilterDefinition<Persona> filter, CancellationToken cancellationToken = default)
    {
        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Persona>> FindAsync(FilterDefinition<Persona> filter, CancellationToken cancellationToken = default)
    {
        return await Collection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<List<Persona>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Collection.Find(Builders<Persona>.Filter.Empty).ToListAsync(cancellationToken);
    }

    public async Task<bool> UpdateByIdAsync(string id, UpdateDefinition<Persona> update, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Persona>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(id));
        var result = await Collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> UpdateOneAsync(FilterDefinition<Persona> filter, UpdateDefinition<Persona> update, CancellationToken cancellationToken = default)
    {
        var result = await Collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public async Task<long> UpdateManyAsync(FilterDefinition<Persona> filter, UpdateDefinition<Persona> update, CancellationToken cancellationToken = default)
    {
        var result = await Collection.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount;
    }

    public async Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Persona>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(id));
        var result = await Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<bool> DeleteOneAsync(FilterDefinition<Persona> filter, CancellationToken cancellationToken = default)
    {
        var result = await Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<long> DeleteManyAsync(FilterDefinition<Persona> filter, CancellationToken cancellationToken = default)
    {
        var result = await Collection.DeleteManyAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount;
    }

    public async Task<long> CountAsync(FilterDefinition<Persona> filter, CancellationToken cancellationToken = default)
    {
        return await Collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
    }

    public async Task<bool> ExistsAsync(FilterDefinition<Persona> filter, CancellationToken cancellationToken = default)
    {
        var options = new CountOptions { Limit = 1 };
        var count = await Collection.CountDocumentsAsync(filter, options, cancellationToken);
        return count > 0;
    }
}