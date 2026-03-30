using MongoDB.Driver;
using PayBille.Api.Models;

namespace PayBille.Api.Infrastructure.Repositories;

/// <summary>
/// Repositorio especializado para operaciones sobre documentos Empresa.
/// </summary>
public sealed class EmpresaRepository : IMongoRepository<Empresa>
{
    private readonly MongoDbContext _dbContext;

    public IMongoCollection<Empresa> Collection => _dbContext.Database.GetCollection<Empresa>("empresas");

    public EmpresaRepository(MongoDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        _dbContext = context;
    }

    /// <summary>
    /// Inserta o reemplaza un documento Empresa buscando por IdEmpresa (upsert).
    /// </summary>
    public async Task UpsertAsync(Empresa empresa, CancellationToken cancellationToken = default)
    {
        var filter  = Builders<Empresa>.Filter.Eq(e => e.IdEmpresa, empresa.IdEmpresa);
        var options = new ReplaceOptions { IsUpsert = true };
        await Collection.ReplaceOneAsync(filter, empresa, options, cancellationToken);
    }

    public async Task InsertOneAsync(Empresa document, CancellationToken cancellationToken = default)
        => await Collection.InsertOneAsync(document, cancellationToken: cancellationToken);

    public async Task InsertManyAsync(IEnumerable<Empresa> documents, CancellationToken cancellationToken = default)
        => await Collection.InsertManyAsync(documents, cancellationToken: cancellationToken);

    public async Task<Empresa?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Empresa>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(id));
        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Empresa?> FindOneAsync(FilterDefinition<Empresa> filter, CancellationToken cancellationToken = default)
        => await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);

    public async Task<List<Empresa>> FindAsync(FilterDefinition<Empresa> filter, CancellationToken cancellationToken = default)
        => await Collection.Find(filter).ToListAsync(cancellationToken);

    public async Task<List<Empresa>> GetAllAsync(CancellationToken cancellationToken = default)
        => await Collection.Find(Builders<Empresa>.Filter.Empty).ToListAsync(cancellationToken);

    public async Task<bool> UpdateByIdAsync(string id, UpdateDefinition<Empresa> update, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Empresa>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(id));
        var result = await Collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> UpdateOneAsync(FilterDefinition<Empresa> filter, UpdateDefinition<Empresa> update, CancellationToken cancellationToken = default)
    {
        var result = await Collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public async Task<long> UpdateManyAsync(FilterDefinition<Empresa> filter, UpdateDefinition<Empresa> update, CancellationToken cancellationToken = default)
    {
        var result = await Collection.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount;
    }

    public async Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Empresa>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(id));
        var result = await Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<bool> DeleteOneAsync(FilterDefinition<Empresa> filter, CancellationToken cancellationToken = default)
    {
        var result = await Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<long> DeleteManyAsync(FilterDefinition<Empresa> filter, CancellationToken cancellationToken = default)
    {
        var result = await Collection.DeleteManyAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount;
    }

    public async Task<long> CountAsync(FilterDefinition<Empresa> filter, CancellationToken cancellationToken = default)
        => await Collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

    public async Task<bool> ExistsAsync(FilterDefinition<Empresa> filter, CancellationToken cancellationToken = default)
    {
        var options = new CountOptions { Limit = 1 };
        var count   = await Collection.CountDocumentsAsync(filter, options, cancellationToken);
        return count > 0;
    }
}
