using MongoDB.Driver;
using PayBille.Api.Common;
using PayBille.Api.DTOs.Empresa;
using PayBille.Api.Errors;
using PayBille.Api.Infrastructure.Repositories;
using PayBille.Api.Interfaces;
using PayBille.Api.Models;

namespace PayBille.Api.Services;

public sealed class EmpresaService : IEmpresaService
{
    private readonly EmpresaRepository _empresaRepository;
    private readonly ILogger<EmpresaService> _logger;

    public EmpresaService(EmpresaRepository empresaRepository, ILogger<EmpresaService> logger)
    {
        _empresaRepository = empresaRepository;
        _logger            = logger;
    }

    /// <inheritdoc/>
    public async Task<Result<List<EmpresaResDto>>> ObtenerTodosAsync(CancellationToken ct)
    {
        var empresas = await _empresaRepository.GetAllAsync(ct);
        return Result<List<EmpresaResDto>>.Ok(empresas.ConvertAll(MapToDto));
    }

    /// <inheritdoc/>
    public async Task<Result<EmpresaResDto>> ObtenerPorIdAsync(string idEmpresa, CancellationToken ct)
    {
        var filter  = Builders<Empresa>.Filter.Eq(e => e.IdEmpresa, idEmpresa);
        var empresa = await _empresaRepository.FindOneAsync(filter, ct);
        return empresa is null
            ? Result<EmpresaResDto>.Fail(AppErrors.EmpresaNoEncontrada(idEmpresa))
            : Result<EmpresaResDto>.Ok(MapToDto(empresa));
    }

    /// <inheritdoc/>
    public async Task<Result<EmpresaResDto>> CrearOActualizarAsync(string? idEmpresa, EmpresaReqDto request, CancellationToken ct)
    {
        Empresa? existente = null;
        if (idEmpresa is not null)
        {
            var filter = Builders<Empresa>.Filter.Eq(e => e.IdEmpresa, idEmpresa);
            existente  = await _empresaRepository.FindOneAsync(filter, ct);
        }

        var empresa = new Empresa
        {
            Id             = existente?.Id ?? MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
            IdEmpresa      = idEmpresa ?? Guid.NewGuid().ToString(),
            Nombre         = request.Nombre,
            Activo         = request.Activo,
            Correo         = request.Correo,
            Descripcion    = request.Descripcion,
            EnlaceCircular = request.EnlaceCircular,
            Direccion      = request.Direccion,
            Direccion2     = request.Direccion2,
            Telefono       = request.Telefono,
            Telefono2      = request.Telefono2,
            IdPropietario  = request.IdPropietario,
            Banco          = request.Banco,
            Imagen         = request.Imagen,
            RNC            = request.RNC,
            ValorImpuesto  = request.ValorImpuesto,
            ZonaHoraria    = request.ZonaHoraria,
            Sucursales     = existente?.Sucursales ?? [],
            CreadoEnUtc    = existente?.CreadoEnUtc ?? DateTime.UtcNow,
        };

        await _empresaRepository.UpsertAsync(empresa, ct);

        _logger.LogInformation(
            "Empresa {IdEmpresa} {Accion}.",
            empresa.IdEmpresa,
            existente is null ? "creada" : "actualizada");

        return Result<EmpresaResDto>.Ok(MapToDto(empresa));
    }

    /// <inheritdoc/>
    public async Task<Result<bool>> EliminarAsync(string idEmpresa, CancellationToken ct)
    {
        var filter    = Builders<Empresa>.Filter.Eq(e => e.IdEmpresa, idEmpresa);
        var eliminado = await _empresaRepository.DeleteOneAsync(filter, ct);
        return eliminado
            ? Result<bool>.Ok(true)
            : Result<bool>.Fail(AppErrors.EmpresaEliminarNoEncontrada(idEmpresa));
    }

    /// <inheritdoc/>
    public async Task<Result<bool>> ActualizarImagenAsync(string idEmpresa, string urlImagen, CancellationToken ct)
    {
        var filter    = Builders<Empresa>.Filter.Eq(e => e.IdEmpresa, idEmpresa);
        var existente = await _empresaRepository.FindOneAsync(filter, ct);
        if (existente is null)
            return Result<bool>.Fail(AppErrors.EmpresaImagenNoEncontrada(idEmpresa));

        existente.Imagen = urlImagen;
        await _empresaRepository.UpsertAsync(existente, ct);

        _logger.LogInformation("Imagen de la empresa {IdEmpresa} actualizada a {Url}.", idEmpresa, urlImagen);
        return Result<bool>.Ok(true);
    }

    /// <inheritdoc/>
    public async Task<Result<EmpresaResDto>> AgregarSucursalAsync(string idEmpresa, SucursalReqDto request, CancellationToken ct)
    {
        var filter    = Builders<Empresa>.Filter.Eq(e => e.IdEmpresa, idEmpresa);
        var existente = await _empresaRepository.FindOneAsync(filter, ct);
        if (existente is null)
            return Result<EmpresaResDto>.Fail(AppErrors.EmpresaNoEncontrada(idEmpresa));

        var sucursal = new Sucursal
        {
            IdSucursal  = Guid.NewGuid().ToString(),
            Nombre      = request.Nombre,
            Direccion   = request.Direccion,
            Direccion2  = request.Direccion2,
            Telefono    = request.Telefono,
            Correo      = request.Correo,
            Estatus     = Models.Enums.EstatusSucursal.Abierta,
            CreadoEnUtc = DateTime.UtcNow,
        };

        existente.Sucursales.Add(sucursal);
        await _empresaRepository.UpsertAsync(existente, ct);

        _logger.LogInformation(
            "Sucursal {IdSucursal} agregada a la empresa {IdEmpresa}.",
            sucursal.IdSucursal,
            idEmpresa);

        return Result<EmpresaResDto>.Ok(MapToDto(existente));
    }

    /// <inheritdoc/>
    public async Task<Result<EmpresaResDto>> ActualizarEstatusSucursalAsync(
        string idEmpresa,
        string idSucursal,
        ActualizarEstatusSucursalReqDto request,
        CancellationToken ct)
    {
        var filter    = Builders<Empresa>.Filter.Eq(e => e.IdEmpresa, idEmpresa);
        var existente = await _empresaRepository.FindOneAsync(filter, ct);
        if (existente is null)
            return Result<EmpresaResDto>.Fail(AppErrors.EmpresaNoEncontrada(idEmpresa));

        var sucursal = existente.Sucursales.FirstOrDefault(s => s.IdSucursal == idSucursal);
        if (sucursal is null)
            return Result<EmpresaResDto>.Fail(AppErrors.SucursalNoEncontrada(idSucursal));

        sucursal.Estatus = request.Estatus;
        await _empresaRepository.UpsertAsync(existente, ct);

        _logger.LogInformation(
            "Estatus de sucursal {IdSucursal} en empresa {IdEmpresa} actualizado a {Estatus}.",
            idSucursal,
            idEmpresa,
            request.Estatus);

        return Result<EmpresaResDto>.Ok(MapToDto(existente));
    }

    /// <inheritdoc/>
    public async Task<Result<bool>> EliminarSucursalAsync(string idEmpresa, string idSucursal, CancellationToken ct)
    {
        var filter    = Builders<Empresa>.Filter.Eq(e => e.IdEmpresa, idEmpresa);
        var existente = await _empresaRepository.FindOneAsync(filter, ct);
        if (existente is null)
            return Result<bool>.Fail(AppErrors.EmpresaNoEncontrada(idEmpresa));

        var sucursal = existente.Sucursales.FirstOrDefault(s => s.IdSucursal == idSucursal);
        if (sucursal is null)
            return Result<bool>.Fail(AppErrors.SucursalEliminarNoEncontrada(idSucursal));

        existente.Sucursales.Remove(sucursal);
        await _empresaRepository.UpsertAsync(existente, ct);

        _logger.LogInformation(
            "Sucursal {IdSucursal} eliminada de la empresa {IdEmpresa}.",
            idSucursal,
            idEmpresa);

        return Result<bool>.Ok(true);
    }

    // ── Mapping ──────────────────────────────────────────────────────────────

    private static EmpresaResDto MapToDto(Empresa e) => new()
    {
        IdEmpresa      = e.IdEmpresa,
        Nombre         = e.Nombre,
        Activo         = e.Activo,
        Correo         = e.Correo,
        Descripcion    = e.Descripcion,
        EnlaceCircular = e.EnlaceCircular,
        Direccion      = e.Direccion,
        Direccion2     = e.Direccion2,
        Telefono       = e.Telefono,
        Telefono2      = e.Telefono2,
        IdPropietario  = e.IdPropietario,
        Banco          = e.Banco,
        Imagen         = e.Imagen,
        RNC            = e.RNC,
        ValorImpuesto  = e.ValorImpuesto,
        ZonaHoraria    = e.ZonaHoraria,
        Sucursales     = e.Sucursales.ConvertAll(MapSucursalToDto),
        CreadoEnUtc    = e.CreadoEnUtc,
    };

    private static DTOs.Empresa.SucursalResDto MapSucursalToDto(Sucursal s) => new()
    {
        IdSucursal  = s.IdSucursal,
        Nombre      = s.Nombre,
        Direccion   = s.Direccion,
        Direccion2  = s.Direccion2,
        Telefono    = s.Telefono,
        Correo      = s.Correo,
        Estatus     = s.Estatus,
        CreadoEnUtc = s.CreadoEnUtc,
    };
}
