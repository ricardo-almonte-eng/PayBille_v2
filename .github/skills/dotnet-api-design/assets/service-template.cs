// ── PayBille v2 Service Template ─────────────────────────────────────────────
// Reglas:
//   - Retornar Result<T>.Ok(valor) en éxito
//   - Retornar Result<T>.Fail(AppErrors.XxxYyy()) en errores de dominio
//   - NUNCA lanzar excepciones para errores esperados
//   - Usar _dbContext convention en repositorios
// ─────────────────────────────────────────────────────────────────────────────
using MongoDB.Driver;
using PayBille.Api.Common;
using PayBille.Api.DTOs.{Entidad};
using PayBille.Api.Errors;
using PayBille.Api.Infrastructure.Repositories;
using PayBille.Api.Interfaces;
using PayBille.Api.Models;

namespace PayBille.Api.Services;

public sealed class {Entidad}Service : I{Entidad}Service
{
    private readonly {Entidad}Repository _{entidad}Repository;
    private readonly ILogger<{Entidad}Service> _logger;

    public {Entidad}Service({Entidad}Repository {entidad}Repository, ILogger<{Entidad}Service> logger)
    {
        _{entidad}Repository = {entidad}Repository;
        _logger              = logger;
    }

    public async Task<Result<List<{Entidad}ResDto>>> ObtenerTodosAsync(CancellationToken ct)
    {
        var items = await _{entidad}Repository.GetAllAsync(ct);
        return Result<List<{Entidad}ResDto>>.Ok(items.ConvertAll(MapToDto));
    }

    public async Task<Result<{Entidad}ResDto>> ObtenerPorIdAsync(string id, CancellationToken ct)
    {
        var filter = Builders<{Entidad}>.Filter.Eq(x => x.Id{Entidad}, id);
        var item   = await _{entidad}Repository.FindOneAsync(filter, ct);
        return item is null
            ? Result<{Entidad}ResDto>.Fail(AppErrors.{Entidad}NoEncontrado(id))
            : Result<{Entidad}ResDto>.Ok(MapToDto(item));
    }

    public async Task<Result<{Entidad}ResDto>> CrearAsync({Entidad}ReqDto request, CancellationToken ct)
    {
        var item = new {Entidad}
        {
            Id{Entidad}    = Guid.NewGuid().ToString(),
            CreadoEnUtc    = DateTime.UtcNow,
            // ... mapear campos del request
        };

        await _{entidad}Repository.UpsertAsync(item, ct);
        _logger.LogInformation("{Entidad} {Id} creado.", item.Id{Entidad});
        return Result<{Entidad}ResDto>.Ok(MapToDto(item));
    }

    public async Task<Result<{Entidad}ResDto>> ActualizarAsync(string id, {Entidad}ReqDto request, CancellationToken ct)
    {
        var filter    = Builders<{Entidad}>.Filter.Eq(x => x.Id{Entidad}, id);
        var existente = await _{entidad}Repository.FindOneAsync(filter, ct);
        if (existente is null)
            return Result<{Entidad}ResDto>.Fail(AppErrors.{Entidad}NoEncontrado(id));

        // ... actualizar campos
        await _{entidad}Repository.UpsertAsync(existente, ct);
        return Result<{Entidad}ResDto>.Ok(MapToDto(existente));
    }

    public async Task<Result<bool>> EliminarAsync(string id, CancellationToken ct)
    {
        var filter    = Builders<{Entidad}>.Filter.Eq(x => x.Id{Entidad}, id);
        var eliminado = await _{entidad}Repository.DeleteOneAsync(filter, ct);
        return eliminado
            ? Result<bool>.Ok(true)
            : Result<bool>.Fail(AppErrors.{Entidad}EliminarNoEncontrado(id));
    }

    // ── Mapping ───────────────────────────────────────────────────────────────

    private static {Entidad}ResDto MapToDto({Entidad} x) => new()
    {
        Id{Entidad} = x.Id{Entidad},
        // ... mapear campos
    };
}
