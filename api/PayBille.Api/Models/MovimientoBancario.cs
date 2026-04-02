using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PayBille.Api.Models.Enums;

namespace PayBille.Api.Models;

/// <summary>
/// Registro de entrada o salida de fondos en una cuenta bancaria.
/// No está sincronizado con el banco real; es un registro manual.
/// </summary>
public sealed class MovimientoBancario
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("idMovimientoBancario")]
    [BsonRequired]
    public string IdMovimientoBancario { get; set; } = string.Empty;

    [BsonElement("idCuentaBancaria")]
    [BsonRequired]
    public string IdCuentaBancaria { get; set; } = string.Empty;

    [BsonElement("idEmpresa")]
    [BsonRequired]
    public string IdEmpresa { get; set; } = string.Empty;

    [BsonElement("fecha")]
    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    [BsonElement("monto")]
    public decimal Monto { get; set; }

    [BsonElement("tipoMovimiento")]
    public TipoMovimientoBancario TipoMovimiento { get; set; }

    [BsonElement("tipoTransferencia")]
    public TipoTransferencia TipoTransferencia { get; set; }

    [BsonElement("referencia")]
    public string? Referencia { get; set; }

    [BsonElement("descripcion")]
    public string? Descripcion { get; set; }

    /// <summary>Venta opcional asociada a este movimiento (ej: pago con transferencia).</summary>
    [BsonElement("idVenta")]
    public string? IdVenta { get; set; }

    [BsonElement("creadoEnUtc")]
    public DateTime CreadoEnUtc { get; set; } = DateTime.UtcNow;
}
