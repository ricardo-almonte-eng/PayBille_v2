namespace PayBille.Api.Models.Enums;

/// <summary>
/// Estatus operativo de una sucursal.
/// </summary>
public enum EstatusSucursal
{
    /// <summary>La sucursal está en operación normal.</summary>
    Abierta = 1,

    /// <summary>La sucursal ha cerrado temporalmente o definitivamente.</summary>
    Cerrada = 2,

    /// <summary>La sucursal fue bloqueada (acceso restringido por administración).</summary>
    Bloqueada = 3,
}
