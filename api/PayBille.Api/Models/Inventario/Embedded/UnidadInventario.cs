using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PayBille.Api.Models.Inventario.Embedded;

/// <summary>
/// Representa una unidad física dentro del inventario de una sucursal.
///
/// MODO ÚNICO (EsUnico = true) — ej: celulares, laptops:
///   Cada entrada es 1 unidad física con su propio CodigoSerial (IMEI, serial, etc).
///   Cantidad siempre es 1.
///   La disponibilidad total = UnidadesInventario.Count(u => !u.Vendida).
///
/// MODO CANTIDAD (EsUnico = false) — ej: cargadores, cables:
///   Una sola entrada con Cantidad = stock disponible.
///   CodigoSerial puede ser null o el código de barra genérico del lote.
///   La disponibilidad total = Unidades.Sum(u => u.Cantidad) (para los no vendidos).
/// </summary>
public sealed class UnidadInventario
{
    [BsonElement("id")]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    /// <summary>
    /// Código único de esta unidad física: IMEI, número de serie, código de barra único.
    /// Requerido cuando EsUnico = true. Opcional para productos en cantidad.
    /// </summary>
    [BsonElement("codigoSerial")]
    public string? CodigoSerial { get; set; }

    /// <summary>
    /// true = esta entrada representa 1 unidad física única (celular, laptop).
    /// false = esta entrada representa N unidades idénticas (cargador x50).
    /// </summary>
    [BsonElement("esUnico")]
    public bool EsUnico { get; set; } = false;

    /// <summary>
    /// Cantidad disponible de esta entrada.
    /// Para EsUnico = true: siempre 1.
    /// Para EsUnico = false: stock real de esta entrada/lote.
    /// </summary>
    [BsonElement("cantidad")]
    public decimal Cantidad { get; set; } = 1;

    /// <summary>Costo de adquisición de esta unidad/lote.</summary>
    [BsonElement("costo")]
    public decimal Costo { get; set; } = 0;

    /// <summary>Estado físico — relevante para productos únicos usados/reacondicionados.</summary>
    [BsonElement("estadoUso")]
    public string? EstadoUso { get; set; }   // "Nuevo", "Como nuevo", "Usado"

    [BsonElement("condicion")]
    public string? Condicion { get; set; }   // "Reacondicionado", "Con detalles"

    [BsonElement("grado")]
    public string? Grado { get; set; }       // "Grado A", "B", "C"

    [BsonElement("bateria")]
    public string? Bateria { get; set; }     // Porcentaje o estado de batería

    [BsonElement("notas")]
    public string? Notas { get; set; }

    /// <summary>true si esta unidad ya fue vendida (solo aplica para EsUnico = true).</summary>
    [BsonElement("vendida")]
    public bool Vendida { get; set; } = false;

    /// <summary>Referencia a la venta que consumió esta unidad.</summary>
    [BsonElement("idVenta")]
    public string? IdVenta { get; set; }

    [BsonElement("nombreCliente")]
    public string? NombreCliente { get; set; }

    /// <summary>true si esta unidad fue recibida por canje/trade-in.</summary>
    [BsonElement("esCanje")]
    public bool EsCanje { get; set; } = false;

    [BsonElement("idUsuarioCanje")]
    public string? IdUsuarioCanje { get; set; }

    /// <summary>Referencia a la compra/entrada por la que llegó esta unidad.</summary>
    [BsonElement("idCompra")]
    public string? IdCompra { get; set; }

    [BsonElement("creadoEnUtc")]
    public DateTime CreadoEnUtc { get; set; } = DateTime.UtcNow;
}
