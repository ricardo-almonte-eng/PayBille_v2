using PayBille.Api.Models.Enums;

namespace PayBille.Api.DTOs.MovimientoBancario;

/// <summary>Datos de respuesta para un movimiento bancario.</summary>
public sealed class MovimientoBancarioResDto
{
    public string                 IdMovimientoBancario         { get; set; } = string.Empty;
    public string                 IdCuentaBancaria             { get; set; } = string.Empty;
    public string                 IdEmpresa                    { get; set; } = string.Empty;
    public DateTime               Fecha                        { get; set; }
    public decimal                Monto                        { get; set; }
    public TipoMovimientoBancario TipoMovimiento               { get; set; }
    public string                 TipoMovimientoDescripcion    => TipoMovimiento.ToString();
    public TipoTransferencia      TipoTransferencia            { get; set; }
    public string                 TipoTransferenciaDescripcion => TipoTransferencia.ToString();
    public string?                Referencia                   { get; set; }
    public string?                Descripcion                  { get; set; }
    public string?                IdVenta                      { get; set; }
    public DateTime               CreadoEnUtc                  { get; set; }
}
