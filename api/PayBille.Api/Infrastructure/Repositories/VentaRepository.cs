using MongoDB.Driver;
using PayBille.Api.Models;

namespace PayBille.Api.Infrastructure.Repositories;

/// <summary>
/// Repositorio especializado para operaciones sobre documentos <see cref="Venta"/>.
/// </summary>
public sealed class VentaRepository : IMongoRepository<Venta>
{
    private readonly MongoDbContext _dbContext;

    public IMongoCollection<Venta> Collection
        => _dbContext.Database.GetCollection<Venta>("ventas");

    public VentaRepository(MongoDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        _dbContext = context;
    }

    public async Task InsertOneAsync(Venta document, CancellationToken cancellationToken = default)
        => await Collection.InsertOneAsync(document, cancellationToken: cancellationToken);

    public async Task InsertManyAsync(IEnumerable<Venta> documents, CancellationToken cancellationToken = default)
        => await Collection.InsertManyAsync(documents, cancellationToken: cancellationToken);

    public async Task<Venta?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Venta>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(id));
        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Venta?> FindOneAsync(FilterDefinition<Venta> filter, CancellationToken cancellationToken = default)
        => await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);

    public async Task<List<Venta>> FindAsync(FilterDefinition<Venta> filter, CancellationToken cancellationToken = default)
        => await Collection.Find(filter).ToListAsync(cancellationToken);

    public async Task<List<Venta>> GetAllAsync(CancellationToken cancellationToken = default)
        => await Collection.Find(Builders<Venta>.Filter.Empty).ToListAsync(cancellationToken);

    public async Task<bool> UpdateByIdAsync(string id, UpdateDefinition<Venta> update, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Venta>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(id));
        var result = await Collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> UpdateOneAsync(FilterDefinition<Venta> filter, UpdateDefinition<Venta> update, CancellationToken cancellationToken = default)
    {
        var result = await Collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public async Task<long> UpdateManyAsync(FilterDefinition<Venta> filter, UpdateDefinition<Venta> update, CancellationToken cancellationToken = default)
    {
        var result = await Collection.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount;
    }

    public async Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Venta>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(id));
        var result = await Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<bool> DeleteOneAsync(FilterDefinition<Venta> filter, CancellationToken cancellationToken = default)
    {
        var result = await Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<long> DeleteManyAsync(FilterDefinition<Venta> filter, CancellationToken cancellationToken = default)
    {
        var result = await Collection.DeleteManyAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount;
    }

    public async Task<long> CountAsync(FilterDefinition<Venta> filter, CancellationToken cancellationToken = default)
        => await Collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

    public async Task<bool> ExistsAsync(FilterDefinition<Venta> filter, CancellationToken cancellationToken = default)
    {
        var options = new CountOptions { Limit = 1 };
        var count   = await Collection.CountDocumentsAsync(filter, options, cancellationToken);
        return count > 0;
    }
}
