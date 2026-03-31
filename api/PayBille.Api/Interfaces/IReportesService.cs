using PayBille.Api.Common;
using PayBille.Api.DTOs.Reportes;

namespace PayBille.Api.Interfaces;

/// <summary>
/// Contrato para el servicio de reportes de la plataforma.
/// Todos los métodos retornan Result&lt;T&gt; — sin excepciones de dominio.
/// </summary>
public interface IReportesService
{
    /// <summary>
    /// Genera el reporte de ventas filtrado por rango de fechas y otros criterios opcionales.
    /// Incluye totales agregados por forma de pago y lista de transacciones.
    /// </summary>
    Task<Result<ReporteVentasResDto>> ObtenerReporteVentasAsync(
        ReporteVentasReqDto filtro, CancellationToken ct);

    /// <summary>
    /// Genera el reporte de inventario de una sucursal.
    /// Incluye totales de unidades y valores, y detecta productos bajo el stock mínimo.
    /// </summary>
    Task<Result<ReporteInventarioResDto>> ObtenerReporteInventarioAsync(
        ReporteInventarioReqDto filtro, CancellationToken ct);

    /// <summary>
    /// Genera el reporte de productos del almacén de la empresa.
    /// Permite filtrar por categoría, marca y estatus activo/inactivo.
    /// </summary>
    Task<Result<ReporteProductosResDto>> ObtenerReporteProductosAsync(
        ReporteProductosReqDto filtro, CancellationToken ct);

    /// <summary>
    /// Genera el reporte de turnos filtrado por rango de fechas y otros criterios opcionales.
    /// Incluye totales de ventas, gastos y diferencias por turno.
    /// </summary>
    Task<Result<ReporteTurnosResDto>> ObtenerReporteTurnosAsync(
        ReporteTurnosReqDto filtro, CancellationToken ct);
}
