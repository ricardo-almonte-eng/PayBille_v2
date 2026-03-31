namespace PayBille.Api.Models.Enums;

/// <summary>
/// Estatus del ciclo de vida de una venta.
/// </summary>
public enum EstatusVenta
{
    /// <summary>Transacción procesada y cobrada exitosamente.</summary>
    Completada = 1,

    /// <summary>Transacción anulada antes o después del cobro.</summary>
    Anulada    = 2,

    /// <summary>Transacción en proceso, pendiente de pago.</summary>
    Pendiente  = 3,

    /// <summary>Mercancía fue devuelta y el pago reembolsado total o parcialmente.</summary>
    Devuelta   = 4,
}
