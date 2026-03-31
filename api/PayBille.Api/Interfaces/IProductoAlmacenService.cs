using PayBille.Api.Common;

namespace PayBille.Api.Interfaces;

/// <summary>
/// Servicio para operaciones sobre productos registrados en el almacén de la empresa.
/// </summary>
public interface IProductoAlmacenService
{
    /// <summary>
    /// Actualiza la imagen del producto de almacén indicado.
    /// Returns Fail(PAU01) si no se encuentra el producto.
    /// </summary>
    Task<Result<bool>> ActualizarImagenAsync(string id, string urlImagen, CancellationToken ct);
}
