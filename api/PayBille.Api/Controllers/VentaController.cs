using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayBille.Api.DTOs;
using PayBille.Api.DTOs.Venta;
using PayBille.Api.Errors;
using PayBille.Api.Interfaces;

namespace PayBille.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class VentaController : ControllerBase
{
    private readonly IVentaService                          _ventaService;
    private readonly IValidator<VentaIngresoRapidoReqDto>   _validatorIngresoRapido;

    public VentaController(
        IVentaService ventaService,
        IValidator<VentaIngresoRapidoReqDto> validatorIngresoRapido)
    {
        _ventaService           = ventaService;
        _validatorIngresoRapido = validatorIngresoRapido;
    }

    /// <summary>
    /// Registra una venta de ingreso rápido con productos ad-hoc.
    /// Permite vender productos que no están registrados en el inventario.
    /// Los ítems se marcan internamente como extras (EsExtra = true).
    /// </summary>
    [HttpPost("ingreso-rapido")]
    [ProducesResponseType(typeof(ApiRespDto<VentaResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> IngresoRapido(
        [FromBody] VentaIngresoRapidoReqDto request,
        CancellationToken cancellationToken)
    {
        var validation = await _validatorIngresoRapido.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            var detalle = string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage));
            return Ok(ApiRespDto<VentaResDto>.Error(AppErrors.VentaValidacionFallida(detalle)));
        }

        var result = await _ventaService.RegistrarIngresoRapidoAsync(request, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<VentaResDto>.Ok(result.Value!)
            : ApiRespDto<VentaResDto>.Error(result.Error!));
    }
}
