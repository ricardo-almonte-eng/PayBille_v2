using PayBille.Api.Models.Enums;

namespace PayBille.Api.DTOs.MovimientoBancario;

/// <summary>Datos de entrada para registrar un movimiento bancario.</summary>
public sealed class MovimientoBancarioReqDto
{
    public string                 IdCuentaBancaria  { get; set; } = string.Empty;
    public string                 IdEmpresa         { get; set; } = string.Empty;
    public DateTime               Fecha             { get; set; } = DateTime.UtcNow;
    public decimal                Monto             { get; set; }
    public TipoMovimientoBancario TipoMovimiento    { get; set; }
    public TipoTransferencia      TipoTransferencia { get; set; }
    public string?                Referencia        { get; set; }
    public string?                Descripcion       { get; set; }
    public string?                IdVenta           { get; set; }
}
