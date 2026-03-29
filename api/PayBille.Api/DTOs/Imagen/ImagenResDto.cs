namespace PayBille.Api.DTOs.Imagen;

/// <summary>
/// Respuesta devuelta tras una subida de imagen exitosa.
/// </summary>
public sealed class ImagenResDto
{
    /// <summary>URL relativa para acceder a la imagen (p. ej. "imagenes/markets/uuid.jpg").</summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>Nombre generado para el archivo en el servidor.</summary>
    public string NombreArchivo { get; set; } = string.Empty;
}
