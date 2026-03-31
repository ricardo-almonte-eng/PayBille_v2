using MongoDB.Driver;
using PayBille.Api.Models.Inventario;

namespace PayBille.Api.Infrastructure.Repositories;

/// <summary>
/// Repositorio especializado para operaciones sobre documentos ProductoAlmacen.
/// </summary>
public sealed class ProductoAlmacenRepository : IMongoRepository<ProductoAlmacen>
{
    private readonly MongoDbContext _dbContext;

    public IMongoCollection<ProductoAlmacen> Collection
        => _dbContext.Database.GetCollection<ProductoAlmacen>("productosAlmacen");

    public ProductoAlmacenRepository(MongoDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        _dbContext = context;
    }

    public async Task InsertOneAsync(ProductoAlmacen document, CancellationToken cancellationToken = default)
        => await Collection.InsertOneAsync(document, cancellationToken: cancellationToken);

    public async Task InsertManyAsync(IEnumerable<ProductoAlmacen> documents, CancellationToken cancellationToken = default)
        => await Collection.InsertManyAsync(documents, cancellationToken: cancellationToken);

    public async Task<ProductoAlmacen?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ProductoAlmacen>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(id));
        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ProductoAlmacen?> FindOneAsync(FilterDefinition<ProductoAlmacen> filter, CancellationToken cancellationToken = default)
        => await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);

    public async Task<List<ProductoAlmacen>> FindAsync(FilterDefinition<ProductoAlmacen> filter, CancellationToken cancellationToken = default)
        => await Collection.Find(filter).ToListAsync(cancellationToken);

    public async Task<List<ProductoAlmacen>> GetAllAsync(CancellationToken cancellationToken = default)
        => await Collection.Find(Builders<ProductoAlmacen>.Filter.Empty).ToListAsync(cancellationToken);

    public async Task<bool> UpdateByIdAsync(string id, UpdateDefinition<ProductoAlmacen> update, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ProductoAlmacen>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(id));
        var result = await Collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> UpdateOneAsync(FilterDefinition<ProductoAlmacen> filter, UpdateDefinition<ProductoAlmacen> update, CancellationToken cancellationToken = default)
    {
        var result = await Collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public async Task<long> UpdateManyAsync(FilterDefinition<ProductoAlmacen> filter, UpdateDefinition<ProductoAlmacen> update, CancellationToken cancellationToken = default)
    {
        var result = await Collection.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount;
    }

    public async Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ProductoAlmacen>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(id));
        var result = await Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<bool> DeleteOneAsync(FilterDefinition<ProductoAlmacen> filter, CancellationToken cancellationToken = default)
    {
        var result = await Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<long> DeleteManyAsync(FilterDefinition<ProductoAlmacen> filter, CancellationToken cancellationToken = default)
    {
        var result = await Collection.DeleteManyAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount;
    }

    public async Task<long> CountAsync(FilterDefinition<ProductoAlmacen> filter, CancellationToken cancellationToken = default)
        => await Collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

    public async Task<bool> ExistsAsync(FilterDefinition<ProductoAlmacen> filter, CancellationToken cancellationToken = default)
    {
        var options = new CountOptions { Limit = 1 };
        var count   = await Collection.CountDocumentsAsync(filter, options, cancellationToken);
        return count > 0;
    }
}
