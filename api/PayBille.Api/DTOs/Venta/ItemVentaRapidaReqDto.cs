namespace PayBille.Api.DTOs.Venta;

/// <summary>
/// Ítem de producto para una venta de ingreso rápido.
/// No requiere referencia al inventario — el producto se describe directamente.
/// </summary>
public sealed class ItemVentaRapidaReqDto
{
    /// <summary>Nombre o descripción del producto vendido.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Cantidad de unidades vendidas. Debe ser mayor a cero.</summary>
    public decimal Cantidad { get; set; }

    /// <summary>Precio unitario del producto.</summary>
    public decimal Precio { get; set; }

    /// <summary>Impuesto aplicado por unidad (ej. ITBIS). Default 0.</summary>
    public decimal Impuesto { get; set; } = 0;

    /// <summary>Descuento fijo aplicado al ítem completo (no por unidad). Default 0.</summary>
    public decimal Descuento { get; set; } = 0;
}
