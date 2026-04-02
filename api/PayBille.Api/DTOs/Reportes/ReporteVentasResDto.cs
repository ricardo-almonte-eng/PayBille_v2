using PayBille.Api.Models.Enums;

namespace PayBille.Api.DTOs.Reportes;

/// <summary>
/// Respuesta del reporte de ventas: totales agregados + lista de transacciones.
/// </summary>
public sealed class ReporteVentasResDto
{
    // ── Parámetros de consulta ──────────────────────────────────────────────
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin    { get; set; }

    // ── Conteos ────────────────────────────────────────────────────────────
    public int TotalVentas      { get; set; }
    public int TotalAnulaciones { get; set; }
    public int TotalDevoluciones { get; set; }
    public int TotalGastos      { get; set; }
    public int TotalItems       { get; set; }

    // ── Montos por forma de pago ───────────────────────────────────────────
    public decimal MontoEfectivo      { get; set; }
    public decimal MontoCredito       { get; set; }
    public decimal MontoDeposito      { get; set; }
    public decimal MontoCanje         { get; set; }
    public decimal MontoFinanciamiento { get; set; }

    // ── Totales fiscales ───────────────────────────────────────────────────
    public decimal SubTotal            { get; set; }
    public decimal TotalImpuestos      { get; set; }
    public decimal TotalDescuentos     { get; set; }
    public decimal TotalBruto          { get; set; }
    public decimal TotalMontoDevuelto  { get; set; }
    public decimal TotalMontoGastos    { get; set; }

    /// <summary>Neto = TotalBruto − TotalMontoDevuelto − TotalMontoGastos.</summary>
    public decimal TotalNeto { get; set; }

    // ── Transacciones ──────────────────────────────────────────────────────
    public List<VentaResumenDto> Ventas { get; set; } = [];
}

/// <summary>Resumen de una transacción individual incluida en el reporte.</summary>
public sealed class VentaResumenDto
{
    public string       Id            { get; set; } = string.Empty;
    public DateTime     Fecha         { get; set; }
    public string?      NombreCliente { get; set; }
    public string?      NombreUsuario { get; set; }
    public string?      IdSucursal    { get; set; }
    public decimal      SubTotal      { get; set; }
    public decimal      Impuesto      { get; set; }
    public decimal      Total         { get; set; }
    public EstatusVenta Estatus       { get; set; }
    public int          CantidadItems { get; set; }
    public bool         EsGasto       { get; set; }
    public string?      TipoGasto     { get; set; }

    // ── Formas de pago ─────────────────────────────────────────────────────
    public decimal Efectivo      { get; set; }
    public decimal Credito       { get; set; }
    public decimal Deposito      { get; set; }
    public decimal Canje         { get; set; }
    public decimal Financiamiento { get; set; }
}
