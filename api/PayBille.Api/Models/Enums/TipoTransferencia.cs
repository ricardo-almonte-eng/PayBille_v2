namespace PayBille.Api.Models.Enums;

/// <summary>Tipo de transferencia o pago bancario.</summary>
public enum TipoTransferencia
{
    Deposito       = 1,
    Transferencia  = 2,
    Cheque         = 3,
    TarjetaDebito  = 4,
    TarjetaCredito = 5,
    Otro           = 6,
}
