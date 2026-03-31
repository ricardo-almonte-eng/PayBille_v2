using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayBille.Api.DTOs;
using PayBille.Api.DTOs.Reportes;
using PayBille.Api.Errors;
using PayBille.Api.Interfaces;

namespace PayBille.Api.Controllers;

/// <summary>
/// Endpoints de reportes: ventas, inventario, productos y turnos.
/// Todos los filtros se reciben como query params (GET).
/// HTTP 200 siempre — éxito y errores de negocio viajan en el body vía ApiRespDto&lt;T&gt;.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class ReportesController : ControllerBase
{
    private readonly IReportesService                       _reportesService;
    private readonly IValidator<ReporteVentasReqDto>        _ventasValidator;
    private readonly IValidator<ReporteInventarioReqDto>    _inventarioValidator;
    private readonly IValidator<ReporteProductosReqDto>     _productosValidator;
    private readonly IValidator<ReporteTurnosReqDto>        _turnosValidator;

    public ReportesController(
        IReportesService                    reportesService,
        IValidator<ReporteVentasReqDto>     ventasValidator,
        IValidator<ReporteInventarioReqDto> inventarioValidator,
        IValidator<ReporteProductosReqDto>  productosValidator,
        IValidator<ReporteTurnosReqDto>     turnosValidator)
    {
        _reportesService      = reportesService;
        _ventasValidator      = ventasValidator;
        _inventarioValidator  = inventarioValidator;
        _productosValidator   = productosValidator;
        _turnosValidator      = turnosValidator;
    }

    /// <summary>
    /// Genera el reporte de ventas filtrado por rango de fechas y criterios opcionales.
    /// Retorna totales agregados por forma de pago y lista de transacciones.
    /// </summary>
    /// <param name="filtro">Filtros de consulta (empresa, fechas, sucursal, persona, estatus).</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    [HttpGet("ventas")]
    [ProducesResponseType(typeof(ApiRespDto<ReporteVentasResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ReporteVentas(
        [FromQuery] ReporteVentasReqDto filtro,
        CancellationToken cancellationToken)
    {
        var validation = await _ventasValidator.ValidateAsync(filtro, cancellationToken);
        if (!validation.IsValid)
        {
            var detalle = string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage));
            return Ok(ApiRespDto<ReporteVentasResDto>.Error(AppErrors.ReporteVentasFiltroInvalido(detalle)));
        }

        var result = await _reportesService.ObtenerReporteVentasAsync(filtro, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<ReporteVentasResDto>.Ok(result.Value!)
            : ApiRespDto<ReporteVentasResDto>.Error(result.Error!));
    }

    /// <summary>
    /// Genera el reporte de inventario de una sucursal.
    /// Incluye totales de unidades y valores, y señala productos bajo el mínimo de stock.
    /// </summary>
    /// <param name="filtro">Filtros de consulta (empresa, sucursal, categoría, bajoStock).</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    [HttpGet("inventario")]
    [ProducesResponseType(typeof(ApiRespDto<ReporteInventarioResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ReporteInventario(
        [FromQuery] ReporteInventarioReqDto filtro,
        CancellationToken cancellationToken)
    {
        var validation = await _inventarioValidator.ValidateAsync(filtro, cancellationToken);
        if (!validation.IsValid)
        {
            var detalle = string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage));
            return Ok(ApiRespDto<ReporteInventarioResDto>.Error(AppErrors.ReporteInventarioFiltroInvalido(detalle)));
        }

        var result = await _reportesService.ObtenerReporteInventarioAsync(filtro, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<ReporteInventarioResDto>.Ok(result.Value!)
            : ApiRespDto<ReporteInventarioResDto>.Error(result.Error!));
    }

    /// <summary>
    /// Genera el reporte de productos del almacén de la empresa.
    /// Permite filtrar por categoría, marca y estatus activo/inactivo.
    /// </summary>
    /// <param name="filtro">Filtros de consulta (empresa, categoría, marca, soloActivos).</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    [HttpGet("productos")]
    [ProducesResponseType(typeof(ApiRespDto<ReporteProductosResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ReporteProductos(
        [FromQuery] ReporteProductosReqDto filtro,
        CancellationToken cancellationToken)
    {
        var validation = await _productosValidator.ValidateAsync(filtro, cancellationToken);
        if (!validation.IsValid)
        {
            var detalle = string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage));
            return Ok(ApiRespDto<ReporteProductosResDto>.Error(AppErrors.ReporteProductosFiltroInvalido(detalle)));
        }

        var result = await _reportesService.ObtenerReporteProductosAsync(filtro, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<ReporteProductosResDto>.Ok(result.Value!)
            : ApiRespDto<ReporteProductosResDto>.Error(result.Error!));
    }

    /// <summary>
    /// Genera el reporte de turnos filtrado por rango de fechas y criterios opcionales.
    /// Incluye totales de ventas, gastos, devoluciones y diferencias por turno.
    /// </summary>
    /// <param name="filtro">Filtros de consulta (empresa, fechas, sucursal, persona, soloCerrados).</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    [HttpGet("turnos")]
    [ProducesResponseType(typeof(ApiRespDto<ReporteTurnosResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ReporteTurnos(
        [FromQuery] ReporteTurnosReqDto filtro,
        CancellationToken cancellationToken)
    {
        var validation = await _turnosValidator.ValidateAsync(filtro, cancellationToken);
        if (!validation.IsValid)
        {
            var detalle = string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage));
            return Ok(ApiRespDto<ReporteTurnosResDto>.Error(AppErrors.ReporteTurnosFiltroInvalido(detalle)));
        }

        var result = await _reportesService.ObtenerReporteTurnosAsync(filtro, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<ReporteTurnosResDto>.Ok(result.Value!)
            : ApiRespDto<ReporteTurnosResDto>.Error(result.Error!));
    }
}
