using PayBille.Api.Common;
using PayBille.Api.DTOs.Venta;

namespace PayBille.Api.Interfaces;

/// <summary>
/// Servicio para operaciones sobre ventas.
/// </summary>
public interface IVentaService
{
    /// <summary>
    /// Registra una venta de ingreso rápido con productos ad-hoc no vinculados al inventario.
    /// Returns Fail(VEC01) si la validación de negocio falla.
    /// Returns Fail(VEC02) si ocurre un error interno al guardar.
    /// </summary>
    Task<Result<VentaResDto>> RegistrarIngresoRapidoAsync(VentaIngresoRapidoReqDto request, CancellationToken ct);
}
