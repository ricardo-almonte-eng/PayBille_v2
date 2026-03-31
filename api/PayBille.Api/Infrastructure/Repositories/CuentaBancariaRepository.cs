using MongoDB.Driver;
using PayBille.Api.Models;

namespace PayBille.Api.Infrastructure.Repositories;

/// <summary>
/// Repositorio especializado para operaciones sobre documentos CuentaBancaria.
/// </summary>
public sealed class CuentaBancariaRepository : IMongoRepository<CuentaBancaria>
{
    private readonly MongoDbContext _dbContext;

    public IMongoCollection<CuentaBancaria> Collection
        => _dbContext.Database.GetCollection<CuentaBancaria>("cuentasBancarias");

    public CuentaBancariaRepository(MongoDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        _dbContext = context;
    }

    /// <summary>
    /// Inserta o reemplaza un documento CuentaBancaria buscando por IdCuentaBancaria (upsert).
    /// </summary>
    public async Task UpsertAsync(CuentaBancaria cuenta, CancellationToken cancellationToken = default)
    {
        var filter  = Builders<CuentaBancaria>.Filter.Eq(c => c.IdCuentaBancaria, cuenta.IdCuentaBancaria);
        var options = new ReplaceOptions { IsUpsert = true };
        await Collection.ReplaceOneAsync(filter, cuenta, options, cancellationToken);
    }

    /// <summary>
    /// Retorna todas las cuentas bancarias pertenecientes a una empresa.
    /// </summary>
    public async Task<List<CuentaBancaria>> FindByEmpresaAsync(string idEmpresa, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CuentaBancaria>.Filter.Eq(c => c.IdEmpresa, idEmpresa);
        return await Collection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task InsertOneAsync(CuentaBancaria document, CancellationToken cancellationToken = default)
        => await Collection.InsertOneAsync(document, cancellationToken: cancellationToken);

    public async Task InsertManyAsync(IEnumerable<CuentaBancaria> documents, CancellationToken cancellationToken = default)
        => await Collection.InsertManyAsync(documents, cancellationToken: cancellationToken);

    public async Task<CuentaBancaria?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CuentaBancaria>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(id));
        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<CuentaBancaria?> FindOneAsync(FilterDefinition<CuentaBancaria> filter, CancellationToken cancellationToken = default)
        => await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);

    public async Task<List<CuentaBancaria>> FindAsync(FilterDefinition<CuentaBancaria> filter, CancellationToken cancellationToken = default)
        => await Collection.Find(filter).ToListAsync(cancellationToken);

    public async Task<List<CuentaBancaria>> GetAllAsync(CancellationToken cancellationToken = default)
        => await Collection.Find(Builders<CuentaBancaria>.Filter.Empty).ToListAsync(cancellationToken);

    public async Task<bool> UpdateByIdAsync(string id, UpdateDefinition<CuentaBancaria> update, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CuentaBancaria>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(id));
        var result = await Collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> UpdateOneAsync(FilterDefinition<CuentaBancaria> filter, UpdateDefinition<CuentaBancaria> update, CancellationToken cancellationToken = default)
    {
        var result = await Collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public async Task<long> UpdateManyAsync(FilterDefinition<CuentaBancaria> filter, UpdateDefinition<CuentaBancaria> update, CancellationToken cancellationToken = default)
    {
        var result = await Collection.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount;
    }

    public async Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CuentaBancaria>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(id));
        var result = await Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<bool> DeleteOneAsync(FilterDefinition<CuentaBancaria> filter, CancellationToken cancellationToken = default)
    {
        var result = await Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<long> DeleteManyAsync(FilterDefinition<CuentaBancaria> filter, CancellationToken cancellationToken = default)
    {
        var result = await Collection.DeleteManyAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount;
    }

    public async Task<long> CountAsync(FilterDefinition<CuentaBancaria> filter, CancellationToken cancellationToken = default)
        => await Collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

    public async Task<bool> ExistsAsync(FilterDefinition<CuentaBancaria> filter, CancellationToken cancellationToken = default)
    {
        var options = new CountOptions { Limit = 1 };
        var count   = await Collection.CountDocumentsAsync(filter, options, cancellationToken);
        return count > 0;
    }
}
