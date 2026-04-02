using MongoDB.Driver;
using PayBille.Api.Models;

namespace PayBille.Api.Infrastructure.Repositories;

/// <summary>
/// Repositorio especializado para operaciones sobre documentos MovimientoBancario.
/// </summary>
public sealed class MovimientoBancarioRepository : IMongoRepository<MovimientoBancario>
{
    private readonly MongoDbContext _dbContext;

    public IMongoCollection<MovimientoBancario> Collection
        => _dbContext.Database.GetCollection<MovimientoBancario>("movimientosBancarios");

    public MovimientoBancarioRepository(MongoDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        _dbContext = context;
    }

    /// <summary>
    /// Inserta o reemplaza un documento MovimientoBancario buscando por IdMovimientoBancario (upsert).
    /// </summary>
    public async Task UpsertAsync(MovimientoBancario movimiento, CancellationToken cancellationToken = default)
    {
        var filter  = Builders<MovimientoBancario>.Filter.Eq(m => m.IdMovimientoBancario, movimiento.IdMovimientoBancario);
        var options = new ReplaceOptions { IsUpsert = true };
        await Collection.ReplaceOneAsync(filter, movimiento, options, cancellationToken);
    }

    /// <summary>
    /// Retorna todos los movimientos pertenecientes a una cuenta bancaria.
    /// </summary>
    public async Task<List<MovimientoBancario>> FindByCuentaAsync(string idCuentaBancaria, CancellationToken cancellationToken = default)
    {
        var filter = Builders<MovimientoBancario>.Filter.Eq(m => m.IdCuentaBancaria, idCuentaBancaria);
        return await Collection.Find(filter)
            .SortByDescending(m => m.Fecha)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Retorna todos los movimientos pertenecientes a una empresa.
    /// </summary>
    public async Task<List<MovimientoBancario>> FindByEmpresaAsync(string idEmpresa, CancellationToken cancellationToken = default)
    {
        var filter = Builders<MovimientoBancario>.Filter.Eq(m => m.IdEmpresa, idEmpresa);
        return await Collection.Find(filter)
            .SortByDescending(m => m.Fecha)
            .ToListAsync(cancellationToken);
    }

    public async Task InsertOneAsync(MovimientoBancario document, CancellationToken cancellationToken = default)
        => await Collection.InsertOneAsync(document, cancellationToken: cancellationToken);

    public async Task InsertManyAsync(IEnumerable<MovimientoBancario> documents, CancellationToken cancellationToken = default)
        => await Collection.InsertManyAsync(documents, cancellationToken: cancellationToken);

    public async Task<MovimientoBancario?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<MovimientoBancario>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(id));
        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<MovimientoBancario?> FindOneAsync(FilterDefinition<MovimientoBancario> filter, CancellationToken cancellationToken = default)
        => await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);

    public async Task<List<MovimientoBancario>> FindAsync(FilterDefinition<MovimientoBancario> filter, CancellationToken cancellationToken = default)
        => await Collection.Find(filter).ToListAsync(cancellationToken);

    public async Task<List<MovimientoBancario>> GetAllAsync(CancellationToken cancellationToken = default)
        => await Collection.Find(Builders<MovimientoBancario>.Filter.Empty).ToListAsync(cancellationToken);

    public async Task<bool> UpdateByIdAsync(string id, UpdateDefinition<MovimientoBancario> update, CancellationToken cancellationToken = default)
    {
        var filter = Builders<MovimientoBancario>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(id));
        var result = await Collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> UpdateOneAsync(FilterDefinition<MovimientoBancario> filter, UpdateDefinition<MovimientoBancario> update, CancellationToken cancellationToken = default)
    {
        var result = await Collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public async Task<long> UpdateManyAsync(FilterDefinition<MovimientoBancario> filter, UpdateDefinition<MovimientoBancario> update, CancellationToken cancellationToken = default)
    {
        var result = await Collection.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount;
    }

    public async Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<MovimientoBancario>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(id));
        var result = await Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<bool> DeleteOneAsync(FilterDefinition<MovimientoBancario> filter, CancellationToken cancellationToken = default)
    {
        var result = await Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<long> DeleteManyAsync(FilterDefinition<MovimientoBancario> filter, CancellationToken cancellationToken = default)
    {
        var result = await Collection.DeleteManyAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount;
    }

    public async Task<long> CountAsync(FilterDefinition<MovimientoBancario> filter, CancellationToken cancellationToken = default)
        => await Collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

    public async Task<bool> ExistsAsync(FilterDefinition<MovimientoBancario> filter, CancellationToken cancellationToken = default)
    {
        var options = new CountOptions { Limit = 1 };
        var count   = await Collection.CountDocumentsAsync(filter, options, cancellationToken);
        return count > 0;
    }
}
