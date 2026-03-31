using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PayBille.Api.Models;

/// <summary>
/// Snapshot diario de totales de ventas por sucursal.
/// Se actualiza automáticamente con cada venta, devolución o gasto para evitar
/// cálculos en tiempo de consulta. Un documento por sucursal por día.
/// </summary>
public sealed class ResumenDiario
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    // ── Clave de partición (empresa + sucursal + fecha) ────────────────────────
    [BsonElement("idEmpresa")]
    public string IdEmpresa { get; set; } = string.Empty;

    [BsonElement("idSucursal")]
    public string IdSucursal { get; set; } = string.Empty;

    /// <summary>
    /// Fecha del resumen en formato ISO (YYYY-MM-DD). Junto con idSucursal forma la clave única.
    /// Se almacena como string para facilitar búsquedas por rango de fechas sin conversiones de zona horaria.
    /// </summary>
    [BsonElement("fecha")]
    public string Fecha { get; set; } = string.Empty;

    // ── Conteos ────────────────────────────────────────────────────────────────
    /// <summary>Número de ventas completadas en el día.</summary>
    [BsonElement("totalVentas")]
    public int TotalVentas { get; set; } = 0;

    /// <summary>Número de ventas anuladas en el día.</summary>
    [BsonElement("totalAnulaciones")]
    public int TotalAnulaciones { get; set; } = 0;

    /// <summary>Número de devoluciones procesadas en el día.</summary>
    [BsonElement("totalDevoluciones")]
    public int TotalDevoluciones { get; set; } = 0;

    /// <summary>Número de gastos/egresos registrados en el día.</summary>
    [BsonElement("totalGastos")]
    public int TotalGastos { get; set; } = 0;

    /// <summary>Número total de ítems (líneas de productos) vendidos en el día.</summary>
    [BsonElement("totalItems")]
    public int TotalItems { get; set; } = 0;

    // ── Montos por forma de pago ───────────────────────────────────────────────
    /// <summary>Suma de todos los pagos recibidos en efectivo.</summary>
    [BsonElement("montoEfectivo")]
    public decimal MontoEfectivo { get; set; } = 0;

    /// <summary>Suma de todos los pagos con tarjeta de crédito/débito.</summary>
    [BsonElement("montoCredito")]
    public decimal MontoCredito { get; set; } = 0;

    /// <summary>Suma de todos los pagos por depósito/transferencia bancaria.</summary>
    [BsonElement("montoDeposito")]
    public decimal MontoDeposito { get; set; } = 0;

    /// <summary>Suma del valor de todos los canjes (trade-ins) recibidos.</summary>
    [BsonElement("montoCanje")]
    public decimal MontoCanje { get; set; } = 0;

    /// <summary>Suma de los montos cubiertos por financiamiento.</summary>
    [BsonElement("montoFinanciamiento")]
    public decimal MontoFinanciamiento { get; set; } = 0;

    // ── Totales fiscales ───────────────────────────────────────────────────────
    /// <summary>Suma de subtotales antes de impuestos.</summary>
    [BsonElement("subTotalDia")]
    public decimal SubTotalDia { get; set; } = 0;

    /// <summary>Suma total de impuestos cobrados en el día.</summary>
    [BsonElement("totalImpuestos")]
    public decimal TotalImpuestos { get; set; } = 0;

    /// <summary>Suma total de descuentos aplicados en el día.</summary>
    [BsonElement("totalDescuentos")]
    public decimal TotalDescuentos { get; set; } = 0;

    /// <summary>Monto bruto total de ventas completadas (sin restar devoluciones ni gastos).</summary>
    [BsonElement("totalBrutoVentas")]
    public decimal TotalBrutoVentas { get; set; } = 0;

    /// <summary>Monto total devuelto a clientes por devoluciones.</summary>
    [BsonElement("totalMontoDevuelto")]
    public decimal TotalMontoDevuelto { get; set; } = 0;

    /// <summary>Monto total de gastos/egresos registrados.</summary>
    [BsonElement("totalMontoGastos")]
    public decimal TotalMontoGastos { get; set; } = 0;

    /// <summary>
    /// Total neto del día = TotalBrutoVentas − TotalMontoDevuelto − TotalMontoGastos.
    /// Calculado y almacenado para evitar operaciones en tiempo de consulta.
    /// </summary>
    [BsonElement("totalNeto")]
    public decimal TotalNeto { get; set; } = 0;

    // ── Metadatos ──────────────────────────────────────────────────────────────
    [BsonElement("creadoEnUtc")]
    public DateTime CreadoEnUtc { get; set; } = DateTime.UtcNow;

    [BsonElement("actualizadoEnUtc")]
    public DateTime ActualizadoEnUtc { get; set; } = DateTime.UtcNow;
}
