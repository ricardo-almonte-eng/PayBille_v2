using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PayBille.Api.Models.Inventario.Embedded;

namespace PayBille.Api.Models.Inventario;

/// <summary>
/// Representa un producto en el inventario de una sucursal.
/// 
/// La propiedad Unidades contiene las unidades físicas disponibles:
///   - Para productos únicos (celulares): N entradas con EsUnico=true, cada una con su CodigoSerial.
///     CantidadDisponible = Unidades.Count(u => !u.Vendida)
///   - Para productos en cantidad (cargadores): 1 entrada con EsUnico=false y Cantidad = stock.
///     CantidadDisponible = Unidades.Sum(u => u.Cantidad)
///
/// Los precios y la garantía son propios de la sucursal — otras sucursales tienen los suyos.
/// </summary>
public sealed class ItemInventarioSucursal
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    // === Referencias ===
    [BsonElement("idEmpresa")]
    public string IdEmpresa { get; set; } = string.Empty;

    [BsonElement("idSucursal")]
    public string IdSucursal { get; set; } = string.Empty;

    /// <summary>Referencia al ProductoAlmacen de la empresa.</summary>
    [BsonElement("idProductoAlmacen")]
    public string IdProductoAlmacen { get; set; } = string.Empty;

    // === Snapshot del producto (para que el historial no se rompa) ===
    [BsonElement("nombreProducto")]
    public string NombreProducto { get; set; } = string.Empty;

    [BsonElement("codigoBarra")]
    public string? CodigoBarra { get; set; }

    [BsonElement("marca")]
    public string? Marca { get; set; }

    [BsonElement("modelo")]
    public string? Modelo { get; set; }

    [BsonElement("categoria")]
    public string? Categoria { get; set; }

    /// <summary>
    /// Si true, cada unidad del producto tiene su propio CodigoSerial (IMEI, serial, etc).
    /// Determina cómo se calculan las cantidades disponibles desde Unidades.
    /// Copiado de ProductoAlmacen.ManejaCodigoUnico al momento de asignar.
    /// </summary>
    [BsonElement("manejaCodigoUnico")]
    public bool ManejaCodigoUnico { get; set; } = false;

    // === Unidades físicas ===
    /// <summary>
    /// Lista de unidades físicas de este producto en esta sucursal.
    /// Ver documentación de UnidadInventario para entender el comportamiento
    /// según ManejaCodigoUnico.
    /// </summary>
    [BsonElement("unidades")]
    public List<UnidadInventario> Unidades { get; set; } = [];

    /// <summary>
    /// Cantidad mínima de stock antes de mostrar alerta de reabastecimiento.
    /// Aplica solo para productos en cantidad (ManejaCodigoUnico = false).
    /// </summary>
    [BsonElement("cantidadMinimaAlerta")]
    public decimal CantidadMinimaAlerta { get; set; } = 1;

    [BsonElement("cantidadInfinita")]
    public bool CantidadInfinita { get; set; } = false;

    // === Precios de la sucursal ===
    [BsonElement("precios")]
    public EsquemaPrecios Precios { get; set; } = new();

    // === Garantía asignada por la sucursal ===
    [BsonElement("garantia")]
    public GarantiaAplicada? Garantia { get; set; }

    // === Proveedor y compra de origen principal ===
    [BsonElement("idProveedor")]
    public string? IdProveedor { get; set; }

    // === Control ===
    [BsonElement("permitirVenta")]
    public bool PermitirVenta { get; set; } = true;

    [BsonElement("ignorarEnReporte")]
    public bool IgnorarEnReporte { get; set; } = false;

    [BsonElement("creadoEnUtc")]
    public DateTime CreadoEnUtc { get; set; } = DateTime.UtcNow;

    [BsonElement("actualizadoEnUtc")]
    public DateTime? ActualizadoEnUtc { get; set; }
}
