using MongoDB.Driver;
using PayBille.Api.Models;

namespace PayBille.Api.Infrastructure.Repositories;

/// <summary>
/// Specialized repository for Market document operations.
/// </summary>
public sealed class MarketRepository : IMongoRepository<Market>
{
    private readonly MongoDbContext _dbContext;

    public IMongoCollection<Market> Collection => _dbContext.Database.GetCollection<Market>("markets");

    public MarketRepository(MongoDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        _dbContext = context;
    }

    /// <summary>
    /// Inserts or replaces a market document matched by IdMarket (upsert).
    /// </summary>
    public async Task UpsertAsync(Market market, CancellationToken cancellationToken = default)
    {
        var filter  = Builders<Market>.Filter.Eq(m => m.IdMarket, market.IdMarket);
        var options = new ReplaceOptions { IsUpsert = true };
        await Collection.ReplaceOneAsync(filter, market, options, cancellationToken);
    }

    // Implement IMongoRepository<Market> methods
    public async Task InsertOneAsync(Market document, CancellationToken cancellationToken = default)
    {
        await Collection.InsertOneAsync(document, cancellationToken: cancellationToken);
    }

    public async Task InsertManyAsync(IEnumerable<Market> documents, CancellationToken cancellationToken = default)
    {
        await Collection.InsertManyAsync(documents, cancellationToken: cancellationToken);
    }

    public async Task<Market?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Market>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(id));
        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Market?> FindOneAsync(FilterDefinition<Market> filter, CancellationToken cancellationToken = default)
    {
        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Market>> FindAsync(FilterDefinition<Market> filter, CancellationToken cancellationToken = default)
    {
        return await Collection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<List<Market>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Collection.Find(Builders<Market>.Filter.Empty).ToListAsync(cancellationToken);
    }

    public async Task<bool> UpdateByIdAsync(string id, UpdateDefinition<Market> update, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Market>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(id));
        var result = await Collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> UpdateOneAsync(FilterDefinition<Market> filter, UpdateDefinition<Market> update, CancellationToken cancellationToken = default)
    {
        var result = await Collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public async Task<long> UpdateManyAsync(FilterDefinition<Market> filter, UpdateDefinition<Market> update, CancellationToken cancellationToken = default)
    {
        var result = await Collection.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount;
    }

    public async Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Market>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(id));
        var result = await Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<bool> DeleteOneAsync(FilterDefinition<Market> filter, CancellationToken cancellationToken = default)
    {
        var result = await Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<long> DeleteManyAsync(FilterDefinition<Market> filter, CancellationToken cancellationToken = default)
    {
        var result = await Collection.DeleteManyAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount;
    }

    public async Task<long> CountAsync(FilterDefinition<Market> filter, CancellationToken cancellationToken = default)
    {
        return await Collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
    }

    public async Task<bool> ExistsAsync(FilterDefinition<Market> filter, CancellationToken cancellationToken = default)
    {
        var options = new CountOptions { Limit = 1 };
        var count = await Collection.CountDocumentsAsync(filter, options, cancellationToken);
        return count > 0;
    }
}
