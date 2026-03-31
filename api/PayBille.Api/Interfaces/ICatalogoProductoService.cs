using PayBille.Api.Common;

namespace PayBille.Api.Interfaces;

/// <summary>
/// Servicio para operaciones sobre productos del catálogo global.
/// </summary>
public interface ICatalogoProductoService
{
    /// <summary>
    /// Actualiza la imagen del producto de catálogo indicado.
    /// Returns Fail(CPU01) si no se encuentra el producto.
    /// </summary>
    Task<Result<bool>> ActualizarImagenAsync(string id, string urlImagen, CancellationToken ct);
}
