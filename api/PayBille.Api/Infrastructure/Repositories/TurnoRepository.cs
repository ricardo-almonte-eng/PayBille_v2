using MongoDB.Driver;
using PayBille.Api.Models;

namespace PayBille.Api.Infrastructure.Repositories;

/// <summary>
/// Repositorio especializado para operaciones sobre documentos <see cref="Turno"/>.
/// </summary>
public sealed class TurnoRepository : IMongoRepository<Turno>
{
    private readonly MongoDbContext _dbContext;

    public IMongoCollection<Turno> Collection
        => _dbContext.Database.GetCollection<Turno>("turnos");

    public TurnoRepository(MongoDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        _dbContext = context;
    }

    public async Task InsertOneAsync(Turno document, CancellationToken cancellationToken = default)
        => await Collection.InsertOneAsync(document, cancellationToken: cancellationToken);

    public async Task InsertManyAsync(IEnumerable<Turno> documents, CancellationToken cancellationToken = default)
        => await Collection.InsertManyAsync(documents, cancellationToken: cancellationToken);

    public async Task<Turno?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Turno>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(id));
        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Turno?> FindOneAsync(FilterDefinition<Turno> filter, CancellationToken cancellationToken = default)
        => await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);

    public async Task<List<Turno>> FindAsync(FilterDefinition<Turno> filter, CancellationToken cancellationToken = default)
        => await Collection.Find(filter).ToListAsync(cancellationToken);

    public async Task<List<Turno>> GetAllAsync(CancellationToken cancellationToken = default)
        => await Collection.Find(Builders<Turno>.Filter.Empty).ToListAsync(cancellationToken);

    public async Task<bool> UpdateByIdAsync(string id, UpdateDefinition<Turno> update, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Turno>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(id));
        var result = await Collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> UpdateOneAsync(FilterDefinition<Turno> filter, UpdateDefinition<Turno> update, CancellationToken cancellationToken = default)
    {
        var result = await Collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public async Task<long> UpdateManyAsync(FilterDefinition<Turno> filter, UpdateDefinition<Turno> update, CancellationToken cancellationToken = default)
    {
        var result = await Collection.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount;
    }

    public async Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Turno>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(id));
        var result = await Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<bool> DeleteOneAsync(FilterDefinition<Turno> filter, CancellationToken cancellationToken = default)
    {
        var result = await Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<long> DeleteManyAsync(FilterDefinition<Turno> filter, CancellationToken cancellationToken = default)
    {
        var result = await Collection.DeleteManyAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount;
    }

    public async Task<long> CountAsync(FilterDefinition<Turno> filter, CancellationToken cancellationToken = default)
        => await Collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

    public async Task<bool> ExistsAsync(FilterDefinition<Turno> filter, CancellationToken cancellationToken = default)
    {
        var options = new CountOptions { Limit = 1 };
        var count   = await Collection.CountDocumentsAsync(filter, options, cancellationToken);
        return count > 0;
    }
}
