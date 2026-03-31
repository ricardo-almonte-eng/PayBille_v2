namespace PayBille.Api.Models.Enums;

/// <summary>
/// Formas de pago aceptadas en una transacción de venta.
/// </summary>
public enum TipoPago
{
    /// <summary>Pago en billetes y monedas.</summary>
    Efectivo       = 1,

    /// <summary>Pago con tarjeta de crédito o débito.</summary>
    Credito        = 2,

    /// <summary>Pago mediante depósito o transferencia bancaria.</summary>
    Deposito       = 3,

    /// <summary>Pago con mercancía entregada en canje (trade-in).</summary>
    Canje          = 4,

    /// <summary>Pago a través de un plan de financiamiento externo.</summary>
    Financiamiento = 5,
}
