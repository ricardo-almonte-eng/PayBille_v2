using MongoDB.Driver;
using PayBille.Api.Common;
using PayBille.Api.DTOs.CuentaBancaria;
using PayBille.Api.Errors;
using PayBille.Api.Infrastructure.Repositories;
using PayBille.Api.Interfaces;
using PayBille.Api.Models;

namespace PayBille.Api.Services;

public sealed class CuentaBancariaService : ICuentaBancariaService
{
    private readonly CuentaBancariaRepository        _cuentaRepository;
    private readonly BancoRepository                 _bancoRepository;
    private readonly MovimientoBancarioRepository    _movimientoRepository;
    private readonly ILogger<CuentaBancariaService>  _logger;

    public CuentaBancariaService(
        CuentaBancariaRepository cuentaRepository,
        BancoRepository bancoRepository,
        MovimientoBancarioRepository movimientoRepository,
        ILogger<CuentaBancariaService> logger)
    {
        _cuentaRepository     = cuentaRepository;
        _bancoRepository      = bancoRepository;
        _movimientoRepository = movimientoRepository;
        _logger               = logger;
    }

    /// <inheritdoc/>
    public async Task<Result<List<CuentaBancariaResDto>>> ObtenerTodosAsync(CancellationToken ct)
    {
        var cuentas = await _cuentaRepository.GetAllAsync(ct);
        return Result<List<CuentaBancariaResDto>>.Ok(cuentas.ConvertAll(MapToDto));
    }

    /// <inheritdoc/>
    public async Task<Result<List<CuentaBancariaResDto>>> ObtenerPorEmpresaAsync(string idEmpresa, CancellationToken ct)
    {
        var cuentas = await _cuentaRepository.FindByEmpresaAsync(idEmpresa, ct);
        return Result<List<CuentaBancariaResDto>>.Ok(cuentas.ConvertAll(MapToDto));
    }

    /// <inheritdoc/>
    public async Task<Result<CuentaBancariaResDto>> ObtenerPorIdAsync(string idCuentaBancaria, CancellationToken ct)
    {
        var filter = Builders<CuentaBancaria>.Filter.Eq(c => c.IdCuentaBancaria, idCuentaBancaria);
        var cuenta = await _cuentaRepository.FindOneAsync(filter, ct);
        return cuenta is null
            ? Result<CuentaBancariaResDto>.Fail(AppErrors.CuentaBancariaNoEncontrada(idCuentaBancaria))
            : Result<CuentaBancariaResDto>.Ok(MapToDto(cuenta));
    }

    /// <inheritdoc/>
    public async Task<Result<CuentaBancariaResDto>> CrearAsync(CuentaBancariaReqDto request, CancellationToken ct)
    {
        // Validar que el banco referenciado exista y obtener su nombre para desnormalizar
        var filtroBanco = Builders<Banco>.Filter.Eq(b => b.IdBanco, request.IdBanco);
        var banco       = await _bancoRepository.FindOneAsync(filtroBanco, ct);
        if (banco is null)
            return Result<CuentaBancariaResDto>.Fail(AppErrors.CuentaBancariaBancoNoEncontrado(request.IdBanco));

        var cuenta = new CuentaBancaria
        {
            Id               = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
            IdCuentaBancaria = Guid.NewGuid().ToString(),
            IdEmpresa        = request.IdEmpresa,
            IdBanco          = request.IdBanco,
            NombreBanco      = banco.Nombre,          // desnormalización
            Nombre           = request.Nombre,
            NumeroCuenta     = request.NumeroCuenta,
            TipoCuenta       = request.TipoCuenta,
            Moneda           = request.Moneda,
            Activo           = request.Activo,
            Descripcion      = request.Descripcion,
            CreadoEnUtc      = DateTime.UtcNow,
        };

        await _cuentaRepository.UpsertAsync(cuenta, ct);

        _logger.LogInformation(
            "CuentaBancaria {IdCuentaBancaria} creada para la empresa {IdEmpresa}.",
            cuenta.IdCuentaBancaria,
            cuenta.IdEmpresa.Replace("\r", "").Replace("\n", ""));

        return Result<CuentaBancariaResDto>.Ok(MapToDto(cuenta));
    }

    /// <inheritdoc/>
    public async Task<Result<CuentaBancariaResDto>> ActualizarAsync(string idCuentaBancaria, CuentaBancariaReqDto request, CancellationToken ct)
    {
        var filter    = Builders<CuentaBancaria>.Filter.Eq(c => c.IdCuentaBancaria, idCuentaBancaria);
        var existente = await _cuentaRepository.FindOneAsync(filter, ct);
        if (existente is null)
            return Result<CuentaBancariaResDto>.Fail(AppErrors.CuentaBancariaNoEncontrada(idCuentaBancaria));

        // Validar y desnormalizar banco si cambió
        var filtroBanco = Builders<Banco>.Filter.Eq(b => b.IdBanco, request.IdBanco);
        var banco       = await _bancoRepository.FindOneAsync(filtroBanco, ct);
        if (banco is null)
            return Result<CuentaBancariaResDto>.Fail(AppErrors.CuentaBancariaBancoNoEncontrado(request.IdBanco));

        // IdEmpresa es inmutable: no se permite mover la cuenta a otra empresa
        existente.IdBanco      = request.IdBanco;
        existente.NombreBanco  = banco.Nombre;
        existente.Nombre       = request.Nombre;
        existente.NumeroCuenta = request.NumeroCuenta;
        existente.TipoCuenta   = request.TipoCuenta;
        existente.Moneda       = request.Moneda;
        existente.Activo       = request.Activo;
        existente.Descripcion  = request.Descripcion;

        await _cuentaRepository.UpsertAsync(existente, ct);

        _logger.LogInformation("CuentaBancaria {IdCuentaBancaria} actualizada.", idCuentaBancaria.Replace("\r", "").Replace("\n", ""));

        return Result<CuentaBancariaResDto>.Ok(MapToDto(existente));
    }

    /// <inheritdoc/>
    public async Task<Result<bool>> EliminarAsync(string idCuentaBancaria, CancellationToken ct)
    {
        // Verificar que no existan movimientos asociados a esta cuenta
        var filtroMovimientos = Builders<MovimientoBancario>.Filter.Eq(m => m.IdCuentaBancaria, idCuentaBancaria);
        var tieneMovimientos  = await _movimientoRepository.ExistsAsync(filtroMovimientos, ct);
        if (tieneMovimientos)
            return Result<bool>.Fail(AppErrors.CuentaBancariaConMovimientosAsociados(idCuentaBancaria));

        var filter    = Builders<CuentaBancaria>.Filter.Eq(c => c.IdCuentaBancaria, idCuentaBancaria);
        var eliminado = await _cuentaRepository.DeleteOneAsync(filter, ct);
        return eliminado
            ? Result<bool>.Ok(true)
            : Result<bool>.Fail(AppErrors.CuentaBancariaEliminarNoEncontrada(idCuentaBancaria));
    }

    // ── Mapping ──────────────────────────────────────────────────────────────

    private static CuentaBancariaResDto MapToDto(CuentaBancaria c) => new()
    {
        IdCuentaBancaria = c.IdCuentaBancaria,
        IdEmpresa        = c.IdEmpresa,
        IdBanco          = c.IdBanco,
        NombreBanco      = c.NombreBanco,
        Nombre           = c.Nombre,
        NumeroCuenta     = c.NumeroCuenta,
        TipoCuenta       = c.TipoCuenta,
        Moneda           = c.Moneda,
        Activo           = c.Activo,
        Descripcion      = c.Descripcion,
        CreadoEnUtc      = c.CreadoEnUtc,
    };
}
