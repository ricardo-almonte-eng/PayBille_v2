using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PayBille.Api.Models.Inventario.Enums;

namespace PayBille.Api.Models.Inventario;

/// <summary>
/// Historial de movimientos de stock — append-only, nunca se modifica.
/// Se genera automáticamente con cada operación que cambie el stock o estado de una unidad.
/// Para productos únicos se registra el CodigoSerial afectado.
/// </summary>
public sealed class MovimientoInventario
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("idEmpresa")]   public string IdEmpresa { get; set; } = string.Empty;
    [BsonElement("idSucursal")]  public string IdSucursal { get; set; } = string.Empty;

    /// <summary>Ref → inventario_sucursal._id</summary>
    [BsonElement("idItem")]      public string IdItem { get; set; } = string.Empty;

    /// <summary>
    /// Para productos únicos: el CodigoSerial (IMEI/serial) de la unidad afectada.
    /// Para productos en cantidad: null.
    /// </summary>
    [BsonElement("codigoSerialAfectado")]
    public string? CodigoSerialAfectado { get; set; }

    // === Snapshot ===
    [BsonElement("nombreProducto")] public string NombreProducto { get; set; } = string.Empty;
    [BsonElement("codigoBarra")]    public string? CodigoBarra { get; set; }

    [BsonElement("tipo")]
    public TipoMovimiento Tipo { get; set; }

    [BsonElement("cantidadAntes")]    public decimal CantidadAntes { get; set; }
    [BsonElement("cantidadDespues")]  public decimal CantidadDespues { get; set; }
    [BsonElement("diferencia")]       public decimal Diferencia { get; set; }

    [BsonElement("idVenta")]   public string? IdVenta { get; set; }
    [BsonElement("idCompra")]  public string? IdCompra { get; set; }
    [BsonElement("idPersona")] public string? IdPersona { get; set; }

    [BsonElement("idUsuario")]      public string IdUsuario { get; set; } = string.Empty;
    [BsonElement("nombreUsuario")]  public string NombreUsuario { get; set; } = string.Empty;
    [BsonElement("comentario")]     public string? Comentario { get; set; }

    [BsonElement("creadoEnUtc")]
    public DateTime CreadoEnUtc { get; set; } = DateTime.UtcNow;
}
