namespace PayBille.Api.DTOs.Reportes;

/// <summary>
/// Respuesta del reporte de inventario: totales + lista de ítems del inventario de la sucursal.
/// </summary>
public sealed class ReporteInventarioResDto
{
    // ── Parámetros de consulta ──────────────────────────────────────────────
    public string IdEmpresa  { get; set; } = string.Empty;
    public string IdSucursal { get; set; } = string.Empty;

    // ── Totales ────────────────────────────────────────────────────────────
    public int     TotalProductos    { get; set; }
    public decimal TotalUnidades     { get; set; }
    public decimal TotalValorCosto   { get; set; }
    public decimal TotalValorVenta   { get; set; }
    public int     TotalBajoStock    { get; set; }

    // ── Ítems ──────────────────────────────────────────────────────────────
    public List<ItemInventarioResumenDto> Items { get; set; } = [];
}

/// <summary>Resumen de un ítem de inventario incluido en el reporte.</summary>
public sealed class ItemInventarioResumenDto
{
    public string   Id                   { get; set; } = string.Empty;
    public string   IdProductoAlmacen    { get; set; } = string.Empty;
    public string   NombreProducto       { get; set; } = string.Empty;
    public string?  Categoria            { get; set; }
    public string?  Marca                { get; set; }
    public string?  Modelo               { get; set; }
    public string?  CodigoBarra          { get; set; }
    public bool     ManejaCodigoUnico    { get; set; }
    public decimal  CantidadDisponible   { get; set; }
    public decimal  CantidadMinimaAlerta { get; set; }
    public bool     BajoStock            { get; set; }
    public bool     CantidadInfinita     { get; set; }
    public decimal  Precio1              { get; set; }
    public decimal  CostoPromedio        { get; set; }
    public bool     PermitirVenta        { get; set; }
}
