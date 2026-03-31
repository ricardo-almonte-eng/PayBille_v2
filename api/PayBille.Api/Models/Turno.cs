using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PayBille.Api.Models;

/// <summary>
/// Representa el turno de trabajo de un usuario en una sucursal.
/// Registra los montos recibidos por forma de pago, los totales esperados y las diferencias.
/// Migrado del modelo relacional <c>Torning</c> de la v1.
/// </summary>
public sealed class Turno
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    // ── Empresa / Sucursal ─────────────────────────────────────────────────────
    [BsonElement("idEmpresa")]
    public string IdEmpresa { get; set; } = string.Empty;

    [BsonElement("idSucursal")]
    public string IdSucursal { get; set; } = string.Empty;

    // ── Persona ────────────────────────────────────────────────────────────────
    [BsonElement("idPersona")]
    public string IdPersona { get; set; } = string.Empty;

    /// <summary>Snapshot del nombre de usuario al abrir el turno.</summary>
    [BsonElement("nombreUsuario")]
    public string NombreUsuario { get; set; } = string.Empty;

    // ── Tiempo ─────────────────────────────────────────────────────────────────
    [BsonElement("inicio")]
    public DateTime Inicio { get; set; } = DateTime.UtcNow;

    /// <summary>Null mientras el turno está activo.</summary>
    [BsonElement("fin")]
    public DateTime? Fin { get; set; }

    // ── Efectivo ───────────────────────────────────────────────────────────────
    /// <summary>Efectivo que había en caja al abrir el turno (fondo de caja inicial).</summary>
    [BsonElement("efectivoEnCaja")]
    public decimal EfectivoEnCaja { get; set; } = 0;

    /// <summary>Total de efectivo esperado según las ventas registradas.</summary>
    [BsonElement("efectivo")]
    public decimal Efectivo { get; set; } = 0;

    /// <summary>Total de efectivo contado físicamente al cerrar el turno.</summary>
    [BsonElement("efectivoRecibido")]
    public decimal EfectivoRecibido { get; set; } = 0;

    // ── Crédito ────────────────────────────────────────────────────────────────
    /// <summary>Total de pagos con tarjeta esperados según las ventas.</summary>
    [BsonElement("credito")]
    public decimal Credito { get; set; } = 0;

    /// <summary>Total de pagos con tarjeta reportados al cerrar.</summary>
    [BsonElement("creditoRecibido")]
    public decimal CreditoRecibido { get; set; } = 0;

    // ── Depósito ───────────────────────────────────────────────────────────────
    /// <summary>Total de depósitos/transferencias esperados.</summary>
    [BsonElement("deposito")]
    public decimal Deposito { get; set; } = 0;

    /// <summary>Total de depósitos/transferencias reportados al cerrar.</summary>
    [BsonElement("depositoRecibido")]
    public decimal DepositoRecibido { get; set; } = 0;

    // ── Canje ──────────────────────────────────────────────────────────────────
    /// <summary>Valor total de canjes (trade-ins) registrados en el turno.</summary>
    [BsonElement("canje")]
    public decimal Canje { get; set; } = 0;

    /// <summary>Valor de canjes reportado por el usuario al cerrar.</summary>
    [BsonElement("canjeRecibido")]
    public decimal CanjeRecibido { get; set; } = 0;

    // ── Totales del turno ──────────────────────────────────────────────────────
    /// <summary>Suma total de efectivo esperado (ventas + fondo inicial − gastos).</summary>
    [BsonElement("totalEfectivo")]
    public decimal TotalEfectivo { get; set; } = 0;

    /// <summary>Suma de todos los pagos no-efectivo (crédito + depósito + canje + financiamiento).</summary>
    [BsonElement("totalOtros")]
    public decimal TotalOtros { get; set; } = 0;

    /// <summary>Resumen serializado de productos vendidos durante el turno.</summary>
    [BsonElement("totalProductos")]
    public string? TotalProductos { get; set; }

    /// <summary>Número total de transacciones procesadas en el turno.</summary>
    [BsonElement("totalTransacciones")]
    public int TotalTransacciones { get; set; } = 0;

    /// <summary>Total de gastos/egresos registrados en el turno.</summary>
    [BsonElement("costos")]
    public decimal Costos { get; set; } = 0;

    /// <summary>Total devuelto por devoluciones de clientes en el turno.</summary>
    [BsonElement("devoluciones")]
    public decimal Devoluciones { get; set; } = 0;

    // ── Faltantes (diferencias al cierre) ─────────────────────────────────────
    /// <summary>Diferencia total (esperado − reportado). Negativo = faltante, Positivo = sobrante.</summary>
    [BsonElement("faltanteTotal")]
    public decimal FaltanteTotal { get; set; } = 0;

    [BsonElement("faltanteEfectivo")]
    public decimal FaltanteEfectivo { get; set; } = 0;

    [BsonElement("faltanteCredito")]
    public decimal FaltanteCredito { get; set; } = 0;

    [BsonElement("faltanteDeposito")]
    public decimal FaltanteDeposito { get; set; } = 0;

    [BsonElement("faltanteCanje")]
    public decimal FaltanteCanje { get; set; } = 0;

    // ── Estatus ────────────────────────────────────────────────────────────────
    [BsonElement("estaCerrado")]
    public bool EstaCerrado { get; set; } = false;

    [BsonElement("creadoEnUtc")]
    public DateTime CreadoEnUtc { get; set; } = DateTime.UtcNow;

    [BsonElement("actualizadoEnUtc")]
    public DateTime? ActualizadoEnUtc { get; set; }
}
