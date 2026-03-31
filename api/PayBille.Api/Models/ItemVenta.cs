using MongoDB.Bson.Serialization.Attributes;

namespace PayBille.Api.Models;

/// <summary>
/// Línea de producto embebida dentro de una <see cref="Venta"/>.
/// Se almacena como snapshot para preservar precios históricos.
/// </summary>
public sealed class ItemVenta
{
    /// <summary>Referencia al ProductoAlmacen original. Null si fue creado manualmente.</summary>
    [BsonElement("idProducto")]
    public string? IdProducto { get; set; }

    /// <summary>Referencia a un combo promocional si aplica.</summary>
    [BsonElement("idCombo")]
    public string? IdCombo { get; set; }

    /// <summary>Referencia al almacén del que salió la unidad.</summary>
    [BsonElement("idAlmacen")]
    public string? IdAlmacen { get; set; }

    /// <summary>Código de barra escaneado al momento de la venta.</summary>
    [BsonElement("codigoBarra")]
    public string? CodigoBarra { get; set; }

    /// <summary>Snapshot del nombre del producto al momento de la venta.</summary>
    [BsonElement("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [BsonElement("cantidad")]
    public decimal Cantidad { get; set; }

    [BsonElement("precio")]
    public decimal Precio { get; set; }

    [BsonElement("impuesto")]
    public decimal Impuesto { get; set; }

    [BsonElement("descuento")]
    public decimal Descuento { get; set; }

    [BsonElement("total")]
    public decimal Total { get; set; }

    /// <summary>Si true, este ítem es mercancía recibida en canje (trade-in).</summary>
    [BsonElement("esCanje")]
    public bool EsCanje { get; set; } = false;

    /// <summary>Si true, el producto se vende al por menor (fraccionado).</summary>
    [BsonElement("esDetalle")]
    public bool EsDetalle { get; set; } = false;

    /// <summary>Tipo de unidad al vender al por menor (ej: "gramo", "ml").</summary>
    [BsonElement("tipoTamanoDetalle")]
    public string? TipoTamanoDetalle { get; set; }

    /// <summary>Cantidad de la unidad de detalle vendida.</summary>
    [BsonElement("tamanoDetalle")]
    public decimal? TamanoDetalle { get; set; }

    /// <summary>Precio unitario de la unidad de detalle.</summary>
    [BsonElement("precioDetalle")]
    public decimal? PrecioDetalle { get; set; }

    /// <summary>Si true, es un cargo extra o servicio adicional no ligado a un producto de inventario.</summary>
    [BsonElement("esExtra")]
    public bool EsExtra { get; set; } = false;
}
