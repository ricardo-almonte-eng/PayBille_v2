namespace PayBille.Api.DTOs.Venta;

/// <summary>
/// Request para registrar una venta de ingreso rápido.
/// Permite vender productos ad-hoc que no están registrados en el inventario.
/// </summary>
public sealed class VentaIngresoRapidoReqDto
{
    // ── Empresa / Sucursal ─────────────────────────────────────────────────────
    public string IdEmpresa  { get; set; } = string.Empty;
    public string IdSucursal { get; set; } = string.Empty;

    // ── Ítems ──────────────────────────────────────────────────────────────────
    /// <summary>Lista de productos a vender. Debe contener al menos un ítem.</summary>
    public List<ItemVentaRapidaReqDto> Items { get; set; } = [];

    // ── Cliente (opcional) ─────────────────────────────────────────────────────
    public string? IdCliente        { get; set; }
    public string? NombreCliente    { get; set; }

    /// <summary>Porcentaje de descuento aplicado al cliente (0–100).</summary>
    public decimal DescuentoCliente { get; set; } = 0;

    // ── Fiscal (opcional) ──────────────────────────────────────────────────────
    public string? Ncf { get; set; }
    public string? Rnc { get; set; }

    // ── Pagos ──────────────────────────────────────────────────────────────────
    public decimal Efectivo      { get; set; } = 0;
    public decimal Credito       { get; set; } = 0;
    public decimal Deposito      { get; set; } = 0;
    public decimal Canje         { get; set; } = 0;
    public decimal Financiamiento { get; set; } = 0;

    // ── Vendedor / Turno (opcional) ────────────────────────────────────────────
    public string? IdPersona    { get; set; }
    public string? NombreUsuario { get; set; }
    public string? IdTurno      { get; set; }

    // ── Extra ──────────────────────────────────────────────────────────────────
    public string? Nota { get; set; }
}
