namespace PayBille.Api.DTOs.Venta;

/// <summary>
/// Representación de un ítem de venta en la respuesta.
/// </summary>
public sealed class ItemVentaResDto
{
    public string? IdProducto { get; set; }
    public string? CodigoBarra { get; set; }
    public string  Nombre      { get; set; } = string.Empty;
    public decimal Cantidad    { get; set; }
    public decimal Precio      { get; set; }
    public decimal Impuesto    { get; set; }
    public decimal Descuento   { get; set; }
    public decimal Total       { get; set; }
    public bool    EsExtra     { get; set; }
}
