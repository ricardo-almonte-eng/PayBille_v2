using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayBille.Api.DTOs;
using PayBille.Api.DTOs.MovimientoBancario;
using PayBille.Api.Errors;
using PayBille.Api.Interfaces;

namespace PayBille.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class MovimientoBancarioController : ControllerBase
{
    private readonly IMovimientoBancarioService           _movimientoService;
    private readonly IValidator<MovimientoBancarioReqDto> _validator;

    public MovimientoBancarioController(
        IMovimientoBancarioService movimientoService,
        IValidator<MovimientoBancarioReqDto> validator)
    {
        _movimientoService = movimientoService;
        _validator         = validator;
    }

    /// <summary>
    /// Obtiene los movimientos bancarios de una cuenta específica. Requiere query param idCuentaBancaria.
    /// </summary>
    [HttpGet("cuenta")]
    [ProducesResponseType(typeof(ApiRespDto<List<MovimientoBancarioResDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerPorCuenta(
        [FromQuery] string idCuentaBancaria,
        CancellationToken cancellationToken)
    {
        var result = await _movimientoService.ObtenerPorCuentaAsync(idCuentaBancaria, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<List<MovimientoBancarioResDto>>.Ok(result.Value!)
            : ApiRespDto<List<MovimientoBancarioResDto>>.Error(result.Error!));
    }

    /// <summary>
    /// Obtiene los movimientos bancarios de una empresa. Requiere query param idEmpresa.
    /// </summary>
    [HttpGet("empresa")]
    [ProducesResponseType(typeof(ApiRespDto<List<MovimientoBancarioResDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerPorEmpresa(
        [FromQuery] string idEmpresa,
        CancellationToken cancellationToken)
    {
        var result = await _movimientoService.ObtenerPorEmpresaAsync(idEmpresa, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<List<MovimientoBancarioResDto>>.Ok(result.Value!)
            : ApiRespDto<List<MovimientoBancarioResDto>>.Error(result.Error!));
    }

    /// <summary>
    /// Obtiene un movimiento bancario por su identificador único (IdMovimientoBancario).
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiRespDto<MovimientoBancarioResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerPorId(string id, CancellationToken cancellationToken)
    {
        var result = await _movimientoService.ObtenerPorIdAsync(id, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<MovimientoBancarioResDto>.Ok(result.Value!)
            : ApiRespDto<MovimientoBancarioResDto>.Error(result.Error!));
    }

    /// <summary>
    /// Registra un nuevo movimiento bancario. El IdMovimientoBancario (GUID) se genera automáticamente en el servidor.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiRespDto<MovimientoBancarioResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Crear(
        [FromBody] MovimientoBancarioReqDto request,
        CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            var detalle = string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage));
            return Ok(ApiRespDto<MovimientoBancarioResDto>.Error(AppErrors.MovimientoBancarioValidacionFallida(detalle)));
        }

        var result = await _movimientoService.CrearAsync(request, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<MovimientoBancarioResDto>.Ok(result.Value!)
            : ApiRespDto<MovimientoBancarioResDto>.Error(result.Error!));
    }

    /// <summary>
    /// Elimina un movimiento bancario por su identificador único (IdMovimientoBancario).
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiRespDto<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Eliminar(string id, CancellationToken cancellationToken)
    {
        var result = await _movimientoService.EliminarAsync(id, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<bool>.Ok(true)
            : ApiRespDto<bool>.Error(result.Error!));
    }
}
