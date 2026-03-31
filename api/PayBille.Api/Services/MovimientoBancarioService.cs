using MongoDB.Driver;
using PayBille.Api.Common;
using PayBille.Api.DTOs.MovimientoBancario;
using PayBille.Api.Errors;
using PayBille.Api.Infrastructure.Repositories;
using PayBille.Api.Interfaces;
using PayBille.Api.Models;

namespace PayBille.Api.Services;

public sealed class MovimientoBancarioService : IMovimientoBancarioService
{
    private readonly MovimientoBancarioRepository        _movimientoRepository;
    private readonly CuentaBancariaRepository            _cuentaRepository;
    private readonly ILogger<MovimientoBancarioService>  _logger;

    public MovimientoBancarioService(
        MovimientoBancarioRepository movimientoRepository,
        CuentaBancariaRepository cuentaRepository,
        ILogger<MovimientoBancarioService> logger)
    {
        _movimientoRepository = movimientoRepository;
        _cuentaRepository     = cuentaRepository;
        _logger               = logger;
    }

    /// <inheritdoc/>
    public async Task<Result<List<MovimientoBancarioResDto>>> ObtenerPorCuentaAsync(string idCuentaBancaria, CancellationToken ct)
    {
        var movimientos = await _movimientoRepository.FindByCuentaAsync(idCuentaBancaria, ct);
        return Result<List<MovimientoBancarioResDto>>.Ok(movimientos.ConvertAll(MapToDto));
    }

    /// <inheritdoc/>
    public async Task<Result<List<MovimientoBancarioResDto>>> ObtenerPorEmpresaAsync(string idEmpresa, CancellationToken ct)
    {
        var movimientos = await _movimientoRepository.FindByEmpresaAsync(idEmpresa, ct);
        return Result<List<MovimientoBancarioResDto>>.Ok(movimientos.ConvertAll(MapToDto));
    }

    /// <inheritdoc/>
    public async Task<Result<MovimientoBancarioResDto>> ObtenerPorIdAsync(string idMovimientoBancario, CancellationToken ct)
    {
        var filter     = Builders<MovimientoBancario>.Filter.Eq(m => m.IdMovimientoBancario, idMovimientoBancario);
        var movimiento = await _movimientoRepository.FindOneAsync(filter, ct);
        return movimiento is null
            ? Result<MovimientoBancarioResDto>.Fail(AppErrors.MovimientoBancarioNoEncontrado(idMovimientoBancario))
            : Result<MovimientoBancarioResDto>.Ok(MapToDto(movimiento));
    }

    /// <inheritdoc/>
    public async Task<Result<MovimientoBancarioResDto>> CrearAsync(MovimientoBancarioReqDto request, CancellationToken ct)
    {
        // Validar que la cuenta bancaria referenciada exista
        var filtroCuenta = Builders<CuentaBancaria>.Filter.Eq(c => c.IdCuentaBancaria, request.IdCuentaBancaria);
        var cuenta       = await _cuentaRepository.FindOneAsync(filtroCuenta, ct);
        if (cuenta is null)
            return Result<MovimientoBancarioResDto>.Fail(AppErrors.MovimientoBancarioCuentaNoEncontrada(request.IdCuentaBancaria));

        var movimiento = new MovimientoBancario
        {
            Id                   = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
            IdMovimientoBancario = Guid.NewGuid().ToString(),
            IdCuentaBancaria     = request.IdCuentaBancaria,
            IdEmpresa            = request.IdEmpresa,
            Fecha                = request.Fecha,
            Monto                = request.Monto,
            TipoMovimiento       = request.TipoMovimiento,
            TipoTransferencia    = request.TipoTransferencia,
            Referencia           = request.Referencia,
            Descripcion          = request.Descripcion,
            IdVenta              = request.IdVenta,
            CreadoEnUtc          = DateTime.UtcNow,
        };

        await _movimientoRepository.UpsertAsync(movimiento, ct);

        _logger.LogInformation(
            "MovimientoBancario {IdMovimientoBancario} creado en cuenta {IdCuentaBancaria}.",
            movimiento.IdMovimientoBancario,
            movimiento.IdCuentaBancaria.Replace("\r", "").Replace("\n", ""));

        return Result<MovimientoBancarioResDto>.Ok(MapToDto(movimiento));
    }

    /// <inheritdoc/>
    public async Task<Result<bool>> EliminarAsync(string idMovimientoBancario, CancellationToken ct)
    {
        var filter    = Builders<MovimientoBancario>.Filter.Eq(m => m.IdMovimientoBancario, idMovimientoBancario);
        var eliminado = await _movimientoRepository.DeleteOneAsync(filter, ct);
        return eliminado
            ? Result<bool>.Ok(true)
            : Result<bool>.Fail(AppErrors.MovimientoBancarioEliminarNoEncontrado(idMovimientoBancario));
    }

    // ── Mapping ──────────────────────────────────────────────────────────────

    private static MovimientoBancarioResDto MapToDto(MovimientoBancario m) => new()
    {
        IdMovimientoBancario = m.IdMovimientoBancario,
        IdCuentaBancaria     = m.IdCuentaBancaria,
        IdEmpresa            = m.IdEmpresa,
        Fecha                = m.Fecha,
        Monto                = m.Monto,
        TipoMovimiento       = m.TipoMovimiento,
        TipoTransferencia    = m.TipoTransferencia,
        Referencia           = m.Referencia,
        Descripcion          = m.Descripcion,
        IdVenta              = m.IdVenta,
        CreadoEnUtc          = m.CreadoEnUtc,
    };
}
