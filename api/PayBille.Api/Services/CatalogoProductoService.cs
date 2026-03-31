using MongoDB.Driver;
using PayBille.Api.Common;
using PayBille.Api.Errors;
using PayBille.Api.Infrastructure.Repositories;
using PayBille.Api.Interfaces;
using PayBille.Api.Models.Catalogo;

namespace PayBille.Api.Services;

/// <summary>
/// Servicio para operaciones sobre productos del catálogo global.
/// </summary>
public sealed class CatalogoProductoService : ICatalogoProductoService
{
    private readonly CatalogoProductoRepository _repo;
    private readonly ILogger<CatalogoProductoService> _logger;

    public CatalogoProductoService(CatalogoProductoRepository repo, ILogger<CatalogoProductoService> logger)
    {
        _repo   = repo;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<Result<bool>> ActualizarImagenAsync(string id, string urlImagen, CancellationToken ct)
    {
        var existente = await _repo.FindByIdAsync(id, ct);
        if (existente is null)
            return Result<bool>.Fail(AppErrors.CatalogoProductoImagenNoEncontrado(id));

        var update = Builders<CatalogoProducto>.Update.Set(p => p.Imagen, urlImagen);
        await _repo.UpdateByIdAsync(id, update, ct);

        _logger.LogInformation("Imagen del producto de catálogo {Id} actualizada a {Url}.", id, urlImagen);
        return Result<bool>.Ok(true);
    }
}
