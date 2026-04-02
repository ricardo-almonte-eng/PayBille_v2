using PayBille.Api.Models.Enums;

namespace PayBille.Api.DTOs.Reportes;

/// <summary>
/// Filtros para generar el reporte de ventas.
/// Se reciben como query params en el endpoint GET /api/reportes/ventas.
/// </summary>
public sealed class ReporteVentasReqDto
{
    /// <summary>Fecha de inicio del rango (UTC). Requerido.</summary>
    public DateTime FechaInicio { get; set; }

    /// <summary>Fecha de fin del rango (UTC). Requerido.</summary>
    public DateTime FechaFin { get; set; }

    /// <summary>Empresa a consultar. Requerido.</summary>
    public string IdEmpresa { get; set; } = string.Empty;

    /// <summary>Sucursal específica. Null = todas las sucursales de la empresa.</summary>
    public string? IdSucursal { get; set; }

    /// <summary>Filtrar por vendedor (persona). Null = todos los vendedores.</summary>
    public string? IdPersona { get; set; }

    /// <summary>Filtrar por estatus de venta. Null = todos los estatus.</summary>
    public EstatusVenta? Estatus { get; set; }

    /// <summary>Incluir gastos/egresos en los resultados. Por defecto false.</summary>
    public bool IncluirGastos { get; set; } = false;
}
