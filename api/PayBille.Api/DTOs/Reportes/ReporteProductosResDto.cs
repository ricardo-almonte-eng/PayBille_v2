namespace PayBille.Api.DTOs.Reportes;

/// <summary>
/// Respuesta del reporte de productos del almacén de la empresa.
/// </summary>
public sealed class ReporteProductosResDto
{
    // ── Totales ────────────────────────────────────────────────────────────
    public int TotalProductos  { get; set; }
    public int TotalActivos    { get; set; }
    public int TotalInactivos  { get; set; }

    // ── Productos ──────────────────────────────────────────────────────────
    public List<ProductoResumenDto> Productos { get; set; } = [];
}

/// <summary>Resumen de un producto del almacén incluido en el reporte.</summary>
public sealed class ProductoResumenDto
{
    public string  Id           { get; set; } = string.Empty;
    public string  Nombre       { get; set; } = string.Empty;
    public string? Descripcion  { get; set; }
    public string? Categoria    { get; set; }
    public string? Marca        { get; set; }
    public string? Modelo       { get; set; }
    public string? CodigoBarra  { get; set; }
    public string? UnidadMedida { get; set; }
    public bool    Activo       { get; set; }
    public bool    EsTaller     { get; set; }
    public bool    EsPieza      { get; set; }
    public bool    EsProducido  { get; set; }
    public bool    ManejaCodigoUnico { get; set; }
    public DateTime CreadoEnUtc { get; set; }
}
