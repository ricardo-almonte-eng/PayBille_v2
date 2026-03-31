using MongoDB.Driver;
using PayBille.Api.Common;
using PayBille.Api.DTOs.Banco;
using PayBille.Api.Errors;
using PayBille.Api.Infrastructure.Repositories;
using PayBille.Api.Interfaces;
using PayBille.Api.Models;

namespace PayBille.Api.Services;

public sealed class BancoService : IBancoService
{
    private readonly BancoRepository              _bancoRepository;
    private readonly CuentaBancariaRepository     _cuentaRepository;
    private readonly ILogger<BancoService>        _logger;

    public BancoService(
        BancoRepository bancoRepository,
        CuentaBancariaRepository cuentaRepository,
        ILogger<BancoService> logger)
    {
        _bancoRepository  = bancoRepository;
        _cuentaRepository = cuentaRepository;
        _logger           = logger;
    }

    /// <inheritdoc/>
    public async Task<Result<List<BancoResDto>>> ObtenerTodosAsync(CancellationToken ct)
    {
        var bancos = await _bancoRepository.GetAllAsync(ct);
        return Result<List<BancoResDto>>.Ok(bancos.ConvertAll(MapToDto));
    }

    /// <inheritdoc/>
    public async Task<Result<BancoResDto>> ObtenerPorIdAsync(string idBanco, CancellationToken ct)
    {
        var filter = Builders<Banco>.Filter.Eq(b => b.IdBanco, idBanco);
        var banco  = await _bancoRepository.FindOneAsync(filter, ct);
        return banco is null
            ? Result<BancoResDto>.Fail(AppErrors.BancoNoEncontrado(idBanco))
            : Result<BancoResDto>.Ok(MapToDto(banco));
    }

    /// <inheritdoc/>
    public async Task<Result<BancoResDto>> CrearAsync(BancoReqDto request, CancellationToken ct)
    {
        // Verificar duplicado de nombre (case insensitive)
        var filtroNombre = Builders<Banco>.Filter.Regex(
            b => b.Nombre,
            new MongoDB.Bson.BsonRegularExpression($"^{System.Text.RegularExpressions.Regex.Escape(request.Nombre)}$", "i"));

        var existente = await _bancoRepository.FindOneAsync(filtroNombre, ct);
        if (existente is not null)
            return Result<BancoResDto>.Fail(AppErrors.BancoNombreDuplicado(request.Nombre));

        var banco = new Banco
        {
            Id          = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
            IdBanco     = Guid.NewGuid().ToString(),
            Nombre      = request.Nombre,
            Codigo      = request.Codigo,
            Activo      = request.Activo,
            CreadoEnUtc = DateTime.UtcNow,
        };

        await _bancoRepository.UpsertAsync(banco, ct);

        _logger.LogInformation("Banco {IdBanco} '{Nombre}' creado.", banco.IdBanco, banco.Nombre.Replace("\r", "").Replace("\n", ""));

        return Result<BancoResDto>.Ok(MapToDto(banco));
    }

    /// <inheritdoc/>
    public async Task<Result<BancoResDto>> ActualizarAsync(string idBanco, BancoReqDto request, CancellationToken ct)
    {
        var filter    = Builders<Banco>.Filter.Eq(b => b.IdBanco, idBanco);
        var existente = await _bancoRepository.FindOneAsync(filter, ct);
        if (existente is null)
            return Result<BancoResDto>.Fail(AppErrors.BancoNoEncontrado(idBanco));

        // Verificar duplicado de nombre excluyendo el banco actual
        var filtroNombre = Builders<Banco>.Filter.And(
            Builders<Banco>.Filter.Regex(
                b => b.Nombre,
                new MongoDB.Bson.BsonRegularExpression($"^{System.Text.RegularExpressions.Regex.Escape(request.Nombre)}$", "i")),
            Builders<Banco>.Filter.Ne(b => b.IdBanco, idBanco));

        var duplicado = await _bancoRepository.FindOneAsync(filtroNombre, ct);
        if (duplicado is not null)
            return Result<BancoResDto>.Fail(AppErrors.BancoNombreDuplicado(request.Nombre));

        existente.Nombre = request.Nombre;
        existente.Codigo = request.Codigo;
        existente.Activo = request.Activo;

        await _bancoRepository.UpsertAsync(existente, ct);

        _logger.LogInformation("Banco {IdBanco} actualizado.", idBanco.Replace("\r", "").Replace("\n", ""));

        return Result<BancoResDto>.Ok(MapToDto(existente));
    }

    /// <inheritdoc/>
    public async Task<Result<bool>> EliminarAsync(string idBanco, CancellationToken ct)
    {
        // Verificar que no existan cuentas bancarias asociadas a este banco
        var filtroCuentas = Builders<CuentaBancaria>.Filter.Eq(c => c.IdBanco, idBanco);
        var tieneCuentas  = await _cuentaRepository.ExistsAsync(filtroCuentas, ct);
        if (tieneCuentas)
            return Result<bool>.Fail(AppErrors.BancoConCuentasAsociadas(idBanco));

        var filter    = Builders<Banco>.Filter.Eq(b => b.IdBanco, idBanco);
        var eliminado = await _bancoRepository.DeleteOneAsync(filter, ct);
        return eliminado
            ? Result<bool>.Ok(true)
            : Result<bool>.Fail(AppErrors.BancoEliminarNoEncontrado(idBanco));
    }

    // ── Mapping ──────────────────────────────────────────────────────────────

    private static BancoResDto MapToDto(Banco b) => new()
    {
        IdBanco     = b.IdBanco,
        Nombre      = b.Nombre,
        Codigo      = b.Codigo,
        Activo      = b.Activo,
        CreadoEnUtc = b.CreadoEnUtc,
    };
}
