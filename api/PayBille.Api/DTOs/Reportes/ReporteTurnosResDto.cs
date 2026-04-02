namespace PayBille.Api.DTOs.Reportes;

/// <summary>
/// Respuesta del reporte de turnos: totales + lista de turnos en el rango.
/// </summary>
public sealed class ReporteTurnosResDto
{
    // ── Parámetros de consulta ──────────────────────────────────────────────
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin    { get; set; }

    // ── Conteos ────────────────────────────────────────────────────────────
    public int TotalTurnos    { get; set; }
    public int TurnosCerrados { get; set; }
    public int TurnosAbiertos { get; set; }

    // ── Totales agregados ──────────────────────────────────────────────────
    public decimal TotalVentasTurnos  { get; set; }
    public decimal TotalGastosTurnos  { get; set; }
    public decimal TotalDevolucioness { get; set; }
    public decimal FaltanteTotalSum   { get; set; }

    // ── Turnos ─────────────────────────────────────────────────────────────
    public List<TurnoResumenDto> Turnos { get; set; } = [];
}

/// <summary>Resumen de un turno incluido en el reporte.</summary>
public sealed class TurnoResumenDto
{
    public string   Id                { get; set; } = string.Empty;
    public string   IdPersona         { get; set; } = string.Empty;
    public string   NombreUsuario     { get; set; } = string.Empty;
    public string   IdSucursal        { get; set; } = string.Empty;
    public DateTime Inicio            { get; set; }
    public DateTime? Fin              { get; set; }
    public bool     EstaCerrado       { get; set; }
    public decimal  EfectivoEnCaja    { get; set; }
    public decimal  TotalEfectivo     { get; set; }
    public decimal  TotalOtros        { get; set; }
    public decimal  Costos            { get; set; }
    public decimal  Devoluciones      { get; set; }
    public int      TotalTransacciones { get; set; }
    public decimal  FaltanteTotal     { get; set; }
    public decimal  FaltanteEfectivo  { get; set; }
    public decimal  FaltanteCredito   { get; set; }
    public decimal  FaltanteDeposito  { get; set; }
}
