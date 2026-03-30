using MongoDB.Driver;
using PayBille.Api.Common;
using PayBille.Api.Errors;
using PayBille.Api.Infrastructure.Repositories;
using PayBille.Api.Interfaces;
using PayBille.Api.Models.Inventario;

namespace PayBille.Api.Services;

/// <summary>
/// Servicio para operaciones sobre productos registrados en el almacén de la empresa.
/// </summary>
public sealed class ProductoAlmacenService : IProductoAlmacenService
{
    private readonly ProductoAlmacenRepository _repo;
    private readonly ILogger<ProductoAlmacenService> _logger;

    public ProductoAlmacenService(ProductoAlmacenRepository repo, ILogger<ProductoAlmacenService> logger)
    {
        _repo   = repo;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<Result<bool>> ActualizarImagenAsync(string id, string urlImagen, CancellationToken ct)
    {
        var existente = await _repo.FindByIdAsync(id, ct);
        if (existente is null)
            return Result<bool>.Fail(AppErrors.ProductoAlmacenImagenNoEncontrado(id));

        var update = Builders<ProductoAlmacen>.Update.Set(p => p.Imagen, urlImagen);
        await _repo.UpdateByIdAsync(id, update, ct);

        _logger.LogInformation("Imagen del producto de almacén {Id} actualizada a {Url}.", id, urlImagen);
        return Result<bool>.Ok(true);
    }
}
