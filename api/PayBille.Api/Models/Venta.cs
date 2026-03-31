using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PayBille.Api.Models.Enums;

namespace PayBille.Api.Models;

/// <summary>
/// Documento principal de una transacción de venta o gasto registrada en una sucursal.
/// Los ítems de la venta se embeben en <see cref="Items"/> para lectura eficiente sin joins.
/// Migrado del modelo relacional <c>Sales + SalesProducts</c> de la v1.
/// </summary>
public sealed class Venta
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    // ── Empresa / Sucursal ─────────────────────────────────────────────────────
    [BsonElement("idEmpresa")]
    public string IdEmpresa { get; set; } = string.Empty;

    [BsonElement("idSucursal")]
    public string IdSucursal { get; set; } = string.Empty;

    // ── Cliente ────────────────────────────────────────────────────────────────
    /// <summary>Referencia a la Persona del cliente. Null si es venta a consumidor general.</summary>
    [BsonElement("idCliente")]
    public string? IdCliente { get; set; }

    /// <summary>Snapshot del nombre del cliente al momento de la venta.</summary>
    [BsonElement("nombreCliente")]
    public string? NombreCliente { get; set; }

    /// <summary>Porcentaje de descuento especial aplicado al cliente.</summary>
    [BsonElement("descuentoCliente")]
    public decimal DescuentoCliente { get; set; } = 0;

    // ── Identificación fiscal ──────────────────────────────────────────────────
    /// <summary>Número de Comprobante Fiscal (NCF) para facturación fiscal.</summary>
    [BsonElement("ncf")]
    public string? Ncf { get; set; }

    /// <summary>Registro Nacional de Contribuyentes del cliente, si aplica.</summary>
    [BsonElement("rnc")]
    public string? Rnc { get; set; }

    // ── Fechas ─────────────────────────────────────────────────────────────────
    /// <summary>Fecha y hora local de la transacción (almacenada en UTC).</summary>
    [BsonElement("fecha")]
    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    [BsonElement("creadoEnUtc")]
    public DateTime CreadoEnUtc { get; set; } = DateTime.UtcNow;

    [BsonElement("actualizadoEnUtc")]
    public DateTime? ActualizadoEnUtc { get; set; }

    // ── Estatus ────────────────────────────────────────────────────────────────
    [BsonElement("estatus")]
    public EstatusVenta Estatus { get; set; } = EstatusVenta.Completada;

    // ── Pagos ──────────────────────────────────────────────────────────────────
    /// <summary>Monto en efectivo recibido del cliente.</summary>
    [BsonElement("efectivo")]
    public decimal Efectivo { get; set; } = 0;

    /// <summary>Cambio devuelto al cliente en efectivo.</summary>
    [BsonElement("cambio")]
    public decimal Cambio { get; set; } = 0;

    /// <summary>Monto cubierto por depósito o transferencia bancaria.</summary>
    [BsonElement("deposito")]
    public decimal Deposito { get; set; } = 0;

    /// <summary>Monto cubierto por tarjeta de crédito o débito.</summary>
    [BsonElement("credito")]
    public decimal Credito { get; set; } = 0;

    /// <summary>Valor de mercancía entregada en canje (trade-in) aplicado al total.</summary>
    [BsonElement("canje")]
    public decimal Canje { get; set; } = 0;

    /// <summary>Monto cubierto por un plan de financiamiento externo.</summary>
    [BsonElement("financiamiento")]
    public decimal Financiamiento { get; set; } = 0;

    /// <summary>Referencia al plan de financiamiento utilizado, si aplica.</summary>
    [BsonElement("idFinanciamiento")]
    public string? IdFinanciamiento { get; set; }

    // ── Totales ────────────────────────────────────────────────────────────────
    [BsonElement("subTotal")]
    public decimal SubTotal { get; set; } = 0;

    [BsonElement("impuesto")]
    public decimal Impuesto { get; set; } = 0;

    [BsonElement("total")]
    public decimal Total { get; set; } = 0;

    // ── Gasto ──────────────────────────────────────────────────────────────────
    /// <summary>Si true, esta transacción representa un egreso/gasto en lugar de una venta.</summary>
    [BsonElement("esGasto")]
    public bool EsGasto { get; set; } = false;

    [BsonElement("tipoGasto")]
    public string? TipoGasto { get; set; }

    // ── Nota ───────────────────────────────────────────────────────────────────
    [BsonElement("nota")]
    public string? Nota { get; set; }

    // ── Vendedor / Turno ───────────────────────────────────────────────────────
    /// <summary>Referencia a la Persona (vendedor) que procesó la transacción.</summary>
    [BsonElement("idPersona")]
    public string? IdPersona { get; set; }

    /// <summary>Snapshot del nombre de usuario del vendedor al momento de la venta.</summary>
    [BsonElement("nombreUsuario")]
    public string? NombreUsuario { get; set; }

    /// <summary>Referencia al turno activo al momento de registrar la venta.</summary>
    [BsonElement("idTurno")]
    public string? IdTurno { get; set; }

    // ── Ítems embebidos ────────────────────────────────────────────────────────
    /// <summary>
    /// Líneas de productos/servicios de la venta, embebidas para lectura eficiente.
    /// Preservan precios y nombres históricos como snapshot inmutable.
    /// </summary>
    [BsonElement("items")]
    public List<ItemVenta> Items { get; set; } = [];
}
