namespace PayBille.Api.Models.Inventario.Enums;

public enum TipoMovimiento
{
    InventarioInicial   = 1,
    EntradaCompra       = 2,
    SalidaVenta         = 3,
    AjustePositivo      = 4,
    AjusteNegativo      = 5,
    EntradaCanje        = 6,
    SalidaCanje         = 7,
    Traslado            = 8,
    DevolucionCliente   = 9,
    DevolucionProveedor = 10
}
