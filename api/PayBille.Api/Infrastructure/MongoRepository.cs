using MongoDB.Bson;
using MongoDB.Driver;

namespace PayBille.Api.Infrastructure;

/// <summary>
/// Generic MongoDB repository implementation for CRUD operations.
/// </summary>
/// <typeparam name="T">The document type</typeparam>
public sealed class MongoRepository<T> : IMongoRepository<T> where T : class
{
    private readonly IMongoCollection<T> _collection;

    public IMongoCollection<T> Collection => _collection;

    public MongoRepository(IMongoCollection<T> collection)
    {
        _collection = collection;
    }

    // ── Create operations ────────────────────────────────────────────────────
    public async Task InsertOneAsync(T document, CancellationToken cancellationToken = default)
    {
        await _collection.InsertOneAsync(document, cancellationToken: cancellationToken);
    }

    public async Task InsertManyAsync(IEnumerable<T> documents, CancellationToken cancellationToken = default)
    {
        await _collection.InsertManyAsync(documents, cancellationToken: cancellationToken);
    }

    // ── Read operations ──────────────────────────────────────────────────────
    public async Task<T?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
        return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<T?> FindOneAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default)
    {
        return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<T>> FindAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default)
    {
        return await _collection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _collection.Find(Builders<T>.Filter.Empty).ToListAsync(cancellationToken);
    }

    // ── Update operations ───────────────────────────────────────────────────
    public async Task<bool> UpdateByIdAsync(string id, UpdateDefinition<T> update, CancellationToken cancellationToken = default)
    {
        var filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
        var result = await _collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> UpdateOneAsync(FilterDefinition<T> filter, UpdateDefinition<T> update, CancellationToken cancellationToken = default)
    {
        var result = await _collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public async Task<long> UpdateManyAsync(FilterDefinition<T> filter, UpdateDefinition<T> update, CancellationToken cancellationToken = default)
    {
        var result = await _collection.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount;
    }

    // ── Delete operations ───────────────────────────────────────────────────
    public async Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
        var result = await _collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<bool> DeleteOneAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default)
    {
        var result = await _collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<long> DeleteManyAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default)
    {
        var result = await _collection.DeleteManyAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount;
    }

    // ── Count and exists ────────────────────────────────────────────────────
    public async Task<long> CountAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default)
    {
        return await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
    }

    public async Task<bool> ExistsAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default)
    {
        var options = new CountOptions { Limit = 1 };
        var count = await _collection.CountDocumentsAsync(filter, options, cancellationToken);
        return count > 0;
    }
}
