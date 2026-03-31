using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayBille.Api.DTOs;
using PayBille.Api.DTOs.CuentaBancaria;
using PayBille.Api.Errors;
using PayBille.Api.Interfaces;

namespace PayBille.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class CuentaBancariaController : ControllerBase
{
    private readonly ICuentaBancariaService          _cuentaService;
    private readonly IValidator<CuentaBancariaReqDto> _validator;

    public CuentaBancariaController(
        ICuentaBancariaService cuentaService,
        IValidator<CuentaBancariaReqDto> validator)
    {
        _cuentaService = cuentaService;
        _validator     = validator;
    }

    /// <summary>
    /// Obtiene la lista completa de cuentas bancarias registradas.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiRespDto<List<CuentaBancariaResDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerTodos(CancellationToken cancellationToken)
    {
        var result = await _cuentaService.ObtenerTodosAsync(cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<List<CuentaBancariaResDto>>.Ok(result.Value!)
            : ApiRespDto<List<CuentaBancariaResDto>>.Error(result.Error!));
    }

    /// <summary>
    /// Obtiene las cuentas bancarias de una empresa. Requiere query param idEmpresa.
    /// </summary>
    [HttpGet("empresa")]
    [ProducesResponseType(typeof(ApiRespDto<List<CuentaBancariaResDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerPorEmpresa(
        [FromQuery] string idEmpresa,
        CancellationToken cancellationToken)
    {
        var result = await _cuentaService.ObtenerPorEmpresaAsync(idEmpresa, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<List<CuentaBancariaResDto>>.Ok(result.Value!)
            : ApiRespDto<List<CuentaBancariaResDto>>.Error(result.Error!));
    }

    /// <summary>
    /// Obtiene una cuenta bancaria por su identificador único (IdCuentaBancaria).
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiRespDto<CuentaBancariaResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerPorId(string id, CancellationToken cancellationToken)
    {
        var result = await _cuentaService.ObtenerPorIdAsync(id, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<CuentaBancariaResDto>.Ok(result.Value!)
            : ApiRespDto<CuentaBancariaResDto>.Error(result.Error!));
    }

    /// <summary>
    /// Registra una nueva cuenta bancaria. El IdCuentaBancaria (GUID) se genera automáticamente en el servidor.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiRespDto<CuentaBancariaResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Crear(
        [FromBody] CuentaBancariaReqDto request,
        CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            var detalle = string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage));
            return Ok(ApiRespDto<CuentaBancariaResDto>.Error(AppErrors.CuentaBancariaValidacionFallida(detalle)));
        }

        var result = await _cuentaService.CrearAsync(request, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<CuentaBancariaResDto>.Ok(result.Value!)
            : ApiRespDto<CuentaBancariaResDto>.Error(result.Error!));
    }

    /// <summary>
    /// Actualiza los datos de una cuenta bancaria existente por su IdCuentaBancaria (GUID).
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiRespDto<CuentaBancariaResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Actualizar(
        string id,
        [FromBody] CuentaBancariaReqDto request,
        CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            var detalle = string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage));
            return Ok(ApiRespDto<CuentaBancariaResDto>.Error(AppErrors.CuentaBancariaValidacionFallida(detalle)));
        }

        var result = await _cuentaService.ActualizarAsync(id, request, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<CuentaBancariaResDto>.Ok(result.Value!)
            : ApiRespDto<CuentaBancariaResDto>.Error(result.Error!));
    }

    /// <summary>
    /// Elimina una cuenta bancaria por su identificador único (IdCuentaBancaria).
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiRespDto<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Eliminar(string id, CancellationToken cancellationToken)
    {
        var result = await _cuentaService.EliminarAsync(id, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<bool>.Ok(true)
            : ApiRespDto<bool>.Error(result.Error!));
    }
}
