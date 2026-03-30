using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayBille.Api.DTOs;
using PayBille.Api.DTOs.Empresa;
using PayBille.Api.DTOs.Imagen;
using PayBille.Api.Errors;
using PayBille.Api.Interfaces;

namespace PayBille.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class EmpresaController : ControllerBase
{
    private readonly IEmpresaService _empresaService;
    private readonly IImagenService  _imagenService;
    private readonly IValidator<EmpresaReqDto> _validator;
    private readonly IValidator<SucursalReqDto> _validatorSucursal;
    private readonly IValidator<ActualizarEstatusSucursalReqDto> _validatorEstatus;

    public EmpresaController(
        IEmpresaService empresaService,
        IImagenService imagenService,
        IValidator<EmpresaReqDto> validator,
        IValidator<SucursalReqDto> validatorSucursal,
        IValidator<ActualizarEstatusSucursalReqDto> validatorEstatus)
    {
        _empresaService    = empresaService;
        _imagenService     = imagenService;
        _validator         = validator;
        _validatorSucursal = validatorSucursal;
        _validatorEstatus  = validatorEstatus;
    }

    /// <summary>
    /// Obtiene la lista completa de empresas registradas.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiRespDto<List<EmpresaResDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerTodos(CancellationToken cancellationToken)
    {
        var result = await _empresaService.ObtenerTodosAsync(cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<List<EmpresaResDto>>.Ok(result.Value!)
            : ApiRespDto<List<EmpresaResDto>>.Error(result.Error!));
    }

    /// <summary>
    /// Obtiene una empresa por su identificador único (IdEmpresa).
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiRespDto<EmpresaResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerPorId(string id, CancellationToken cancellationToken)
    {
        var result = await _empresaService.ObtenerPorIdAsync(id, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<EmpresaResDto>.Ok(result.Value!)
            : ApiRespDto<EmpresaResDto>.Error(result.Error!));
    }

    /// <summary>
    /// Registra una nueva empresa. El IdEmpresa (GUID) se genera automáticamente en el servidor.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiRespDto<EmpresaResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Crear(
        [FromBody] EmpresaReqDto request,
        CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            var detalle = string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage));
            return Ok(ApiRespDto<EmpresaResDto>.Error(AppErrors.EmpresaValidacionFallida(detalle)));
        }

        var result = await _empresaService.CrearOActualizarAsync(null, request, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<EmpresaResDto>.Ok(result.Value!)
            : ApiRespDto<EmpresaResDto>.Error(result.Error!));
    }

    /// <summary>
    /// Actualiza los datos de una empresa existente por su IdEmpresa (GUID).
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiRespDto<EmpresaResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Actualizar(
        string id,
        [FromBody] EmpresaReqDto request,
        CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            var detalle = string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage));
            return Ok(ApiRespDto<EmpresaResDto>.Error(AppErrors.EmpresaValidacionFallida(detalle)));
        }

        var result = await _empresaService.CrearOActualizarAsync(id, request, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<EmpresaResDto>.Ok(result.Value!)
            : ApiRespDto<EmpresaResDto>.Error(result.Error!));
    }

    /// <summary>
    /// Elimina una empresa por su identificador único (IdEmpresa).
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiRespDto<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Eliminar(string id, CancellationToken cancellationToken)
    {
        var result = await _empresaService.EliminarAsync(id, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<bool>.Ok(true)
            : ApiRespDto<bool>.Error(result.Error!));
    }

    /// <summary>
    /// Sube y asocia una imagen a la empresa indicada por IdEmpresa.
    /// El archivo debe enviarse como multipart/form-data en el campo "archivo".
    /// </summary>
    [HttpPost("{id}/imagen")]
    [ProducesResponseType(typeof(ApiRespDto<ImagenResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SubirImagen(
        string id,
        IFormFile archivo,
        CancellationToken cancellationToken)
    {
        var resultImagen = await _imagenService.SubirAsync(archivo, "empresas", cancellationToken);
        if (!resultImagen.IsSuccess)
            return Ok(ApiRespDto<ImagenResDto>.Error(resultImagen.Error!));

        var resultActualizar = await _empresaService.ActualizarImagenAsync(id, resultImagen.Value!.Url, cancellationToken);
        return Ok(resultActualizar.IsSuccess
            ? ApiRespDto<ImagenResDto>.Ok(resultImagen.Value!)
            : ApiRespDto<ImagenResDto>.Error(resultActualizar.Error!));
    }

    // ── Sucursales ────────────────────────────────────────────────────────────

    /// <summary>
    /// Agrega una nueva sucursal a la empresa indicada. El IdSucursal se genera en el servidor.
    /// </summary>
    [HttpPost("{id}/sucursales")]
    [ProducesResponseType(typeof(ApiRespDto<EmpresaResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> AgregarSucursal(
        string id,
        [FromBody] SucursalReqDto request,
        CancellationToken cancellationToken)
    {
        var validation = await _validatorSucursal.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            var detalle = string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage));
            return Ok(ApiRespDto<EmpresaResDto>.Error(AppErrors.SucursalValidacionFallida(detalle)));
        }

        var result = await _empresaService.AgregarSucursalAsync(id, request, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<EmpresaResDto>.Ok(result.Value!)
            : ApiRespDto<EmpresaResDto>.Error(result.Error!));
    }

    /// <summary>
    /// Actualiza el estatus de una sucursal (Abierta, Cerrada, Bloqueada).
    /// </summary>
    [HttpPut("{id}/sucursales/{idSucursal}/estatus")]
    [ProducesResponseType(typeof(ApiRespDto<EmpresaResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ActualizarEstatusSucursal(
        string id,
        string idSucursal,
        [FromBody] ActualizarEstatusSucursalReqDto request,
        CancellationToken cancellationToken)
    {
        var validation = await _validatorEstatus.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            var detalle = string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage));
            return Ok(ApiRespDto<EmpresaResDto>.Error(AppErrors.SucursalEstatusValidacionFallida(detalle)));
        }

        var result = await _empresaService.ActualizarEstatusSucursalAsync(id, idSucursal, request, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<EmpresaResDto>.Ok(result.Value!)
            : ApiRespDto<EmpresaResDto>.Error(result.Error!));
    }

    /// <summary>
    /// Elimina una sucursal de la empresa indicada.
    /// </summary>
    [HttpDelete("{id}/sucursales/{idSucursal}")]
    [ProducesResponseType(typeof(ApiRespDto<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> EliminarSucursal(
        string id,
        string idSucursal,
        CancellationToken cancellationToken)
    {
        var result = await _empresaService.EliminarSucursalAsync(id, idSucursal, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<bool>.Ok(true)
            : ApiRespDto<bool>.Error(result.Error!));
    }
}
