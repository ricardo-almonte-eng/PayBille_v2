namespace PayBille.Api.DTOs.Reportes;

/// <summary>
/// Filtros para generar el reporte de inventario de una sucursal.
/// Se reciben como query params en el endpoint GET /api/reportes/inventario.
/// </summary>
public sealed class ReporteInventarioReqDto
{
    /// <summary>Empresa a consultar. Requerido.</summary>
    public string IdEmpresa { get; set; } = string.Empty;

    /// <summary>Sucursal a consultar. Requerido.</summary>
    public string IdSucursal { get; set; } = string.Empty;

    /// <summary>Filtrar por categoría de producto. Null = todas las categorías.</summary>
    public string? Categoria { get; set; }

    /// <summary>Si true, retorna solo los productos con stock por debajo de la cantidad mínima de alerta.</summary>
    public bool? BajoStock { get; set; }

    /// <summary>Si true (por defecto), excluye los productos marcados con IgnorarEnReporte.</summary>
    public bool ExcluirIgnorados { get; set; } = true;
}
