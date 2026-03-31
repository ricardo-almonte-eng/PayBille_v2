using MongoDB.Driver;
using PayBille.Api.Models;

namespace PayBille.Api.Infrastructure.Repositories;

/// <summary>
/// Repositorio especializado para operaciones sobre documentos <see cref="ResumenDiario"/>.
/// Soporta upsert atómico por (idSucursal + fecha) para actualización incremental de totales.
/// </summary>
public sealed class ResumenDiarioRepository : IMongoRepository<ResumenDiario>
{
    private readonly MongoDbContext _dbContext;

    public IMongoCollection<ResumenDiario> Collection
        => _dbContext.Database.GetCollection<ResumenDiario>("resumenesDiarios");

    public ResumenDiarioRepository(MongoDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        _dbContext = context;
    }

    /// <summary>
    /// Aplica incrementos atómicos sobre el resumen del día para la sucursal indicada.
    /// Si no existe el documento para esa fecha, lo crea (upsert).
    /// </summary>
    public async Task AplicarIncrementosAsync(
        string idEmpresa,
        string idSucursal,
        string fecha,
        UpdateDefinition<ResumenDiario> incrementos,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<ResumenDiario>.Filter.And(
            Builders<ResumenDiario>.Filter.Eq(r => r.IdEmpresa,  idEmpresa),
            Builders<ResumenDiario>.Filter.Eq(r => r.IdSucursal, idSucursal),
            Builders<ResumenDiario>.Filter.Eq(r => r.Fecha,      fecha));

        var options = new UpdateOptions { IsUpsert = true };
        await Collection.UpdateOneAsync(filter, incrementos, options, cancellationToken);
    }

    public async Task InsertOneAsync(ResumenDiario document, CancellationToken cancellationToken = default)
        => await Collection.InsertOneAsync(document, cancellationToken: cancellationToken);

    public async Task InsertManyAsync(IEnumerable<ResumenDiario> documents, CancellationToken cancellationToken = default)
        => await Collection.InsertManyAsync(documents, cancellationToken: cancellationToken);

    public async Task<ResumenDiario?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ResumenDiario>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(id));
        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ResumenDiario?> FindOneAsync(FilterDefinition<ResumenDiario> filter, CancellationToken cancellationToken = default)
        => await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);

    public async Task<List<ResumenDiario>> FindAsync(FilterDefinition<ResumenDiario> filter, CancellationToken cancellationToken = default)
        => await Collection.Find(filter).ToListAsync(cancellationToken);

    public async Task<List<ResumenDiario>> GetAllAsync(CancellationToken cancellationToken = default)
        => await Collection.Find(Builders<ResumenDiario>.Filter.Empty).ToListAsync(cancellationToken);

    public async Task<bool> UpdateByIdAsync(string id, UpdateDefinition<ResumenDiario> update, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ResumenDiario>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(id));
        var result = await Collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> UpdateOneAsync(FilterDefinition<ResumenDiario> filter, UpdateDefinition<ResumenDiario> update, CancellationToken cancellationToken = default)
    {
        var result = await Collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public async Task<long> UpdateManyAsync(FilterDefinition<ResumenDiario> filter, UpdateDefinition<ResumenDiario> update, CancellationToken cancellationToken = default)
    {
        var result = await Collection.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount;
    }

    public async Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ResumenDiario>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(id));
        var result = await Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<bool> DeleteOneAsync(FilterDefinition<ResumenDiario> filter, CancellationToken cancellationToken = default)
    {
        var result = await Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<long> DeleteManyAsync(FilterDefinition<ResumenDiario> filter, CancellationToken cancellationToken = default)
    {
        var result = await Collection.DeleteManyAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount;
    }

    public async Task<long> CountAsync(FilterDefinition<ResumenDiario> filter, CancellationToken cancellationToken = default)
        => await Collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

    public async Task<bool> ExistsAsync(FilterDefinition<ResumenDiario> filter, CancellationToken cancellationToken = default)
    {
        var options = new CountOptions { Limit = 1 };
        var count   = await Collection.CountDocumentsAsync(filter, options, cancellationToken);
        return count > 0;
    }
}
