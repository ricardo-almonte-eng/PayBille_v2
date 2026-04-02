namespace PayBille.Api.DTOs.Reportes;

/// <summary>
/// Filtros para generar el reporte de productos de la empresa.
/// Se reciben como query params en el endpoint GET /api/reportes/productos.
/// </summary>
public sealed class ReporteProductosReqDto
{
    /// <summary>Empresa a consultar. Requerido.</summary>
    public string IdEmpresa { get; set; } = string.Empty;

    /// <summary>Filtrar por categoría. Null = todas las categorías.</summary>
    public string? Categoria { get; set; }

    /// <summary>Filtrar por marca. Null = todas las marcas.</summary>
    public string? Marca { get; set; }

    /// <summary>Si true (por defecto), retorna solo los productos activos.</summary>
    public bool SoloActivos { get; set; } = true;
}
