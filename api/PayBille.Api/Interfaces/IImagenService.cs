using PayBille.Api.Common;
using PayBille.Api.DTOs.Imagen;

namespace PayBille.Api.Interfaces;

/// <summary>
/// Servicio reutilizable para subir imágenes al sistema de archivos.
/// El directorio base es configurable a través de <c>ImagenSettings</c>.
/// </summary>
public interface IImagenService
{
    /// <summary>
    /// Valida y guarda el archivo en <c>DirectorioBase/{subDirectorio}/</c>.
    /// Returns Fail(IMS01) si el archivo es nulo o vacío.
    /// Returns Fail(IMS02) si el tipo MIME no está permitido.
    /// Returns Fail(IMS03) si el tamaño supera el máximo configurado.
    /// Returns Fail(IMS04) si ocurre un error de I/O al guardar.
    /// </summary>
    /// <param name="archivo">Archivo recibido desde el formulario multipart.</param>
    /// <param name="subDirectorio">Subdirectorio dentro de <c>DirectorioBase</c> (p. ej. "markets", "personas").</param>
    /// <param name="ct">Token de cancelación.</param>
    Task<Result<ImagenResDto>> SubirAsync(IFormFile archivo, string subDirectorio, CancellationToken ct);
}
