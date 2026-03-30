using MongoDB.Driver;
using PayBille.Api.Models.Catalogo;

namespace PayBille.Api.Infrastructure.Repositories;

/// <summary>
/// Repositorio especializado para operaciones sobre documentos CatalogoProducto.
/// </summary>
public sealed class CatalogoProductoRepository : IMongoRepository<CatalogoProducto>
{
    private readonly MongoDbContext _dbContext;

    public IMongoCollection<CatalogoProducto> Collection
        => _dbContext.Database.GetCollection<CatalogoProducto>("catalogoProductos");

    public CatalogoProductoRepository(MongoDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        _dbContext = context;
    }

    public async Task InsertOneAsync(CatalogoProducto document, CancellationToken cancellationToken = default)
        => await Collection.InsertOneAsync(document, cancellationToken: cancellationToken);

    public async Task InsertManyAsync(IEnumerable<CatalogoProducto> documents, CancellationToken cancellationToken = default)
        => await Collection.InsertManyAsync(documents, cancellationToken: cancellationToken);

    public async Task<CatalogoProducto?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CatalogoProducto>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(id));
        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<CatalogoProducto?> FindOneAsync(FilterDefinition<CatalogoProducto> filter, CancellationToken cancellationToken = default)
        => await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);

    public async Task<List<CatalogoProducto>> FindAsync(FilterDefinition<CatalogoProducto> filter, CancellationToken cancellationToken = default)
        => await Collection.Find(filter).ToListAsync(cancellationToken);

    public async Task<List<CatalogoProducto>> GetAllAsync(CancellationToken cancellationToken = default)
        => await Collection.Find(Builders<CatalogoProducto>.Filter.Empty).ToListAsync(cancellationToken);

    public async Task<bool> UpdateByIdAsync(string id, UpdateDefinition<CatalogoProducto> update, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CatalogoProducto>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(id));
        var result = await Collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> UpdateOneAsync(FilterDefinition<CatalogoProducto> filter, UpdateDefinition<CatalogoProducto> update, CancellationToken cancellationToken = default)
    {
        var result = await Collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public async Task<long> UpdateManyAsync(FilterDefinition<CatalogoProducto> filter, UpdateDefinition<CatalogoProducto> update, CancellationToken cancellationToken = default)
    {
        var result = await Collection.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount;
    }

    public async Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CatalogoProducto>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(id));
        var result = await Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<bool> DeleteOneAsync(FilterDefinition<CatalogoProducto> filter, CancellationToken cancellationToken = default)
    {
        var result = await Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<long> DeleteManyAsync(FilterDefinition<CatalogoProducto> filter, CancellationToken cancellationToken = default)
    {
        var result = await Collection.DeleteManyAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount;
    }

    public async Task<long> CountAsync(FilterDefinition<CatalogoProducto> filter, CancellationToken cancellationToken = default)
        => await Collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

    public async Task<bool> ExistsAsync(FilterDefinition<CatalogoProducto> filter, CancellationToken cancellationToken = default)
    {
        var options = new CountOptions { Limit = 1 };
        var count   = await Collection.CountDocumentsAsync(filter, options, cancellationToken);
        return count > 0;
    }
}
