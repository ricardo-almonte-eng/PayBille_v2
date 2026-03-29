namespace PayBille.Api.Configuration;

/// <summary>
/// Configuración para el servicio de subida de imágenes.
/// Permite ajustar el directorio base, el tamaño máximo permitido y los tipos MIME aceptados.
/// </summary>
public sealed class ImagenSettings
{
    public const string SectionName = "Imagenes";

    /// <summary>
    /// Directorio base donde se almacenan las imágenes.
    /// Puede ser una ruta absoluta o relativa a la raíz de contenido de la aplicación.
    /// </summary>
    public string DirectorioBase { get; set; } = "uploads";

    /// <summary>Tamaño máximo de archivo permitido en bytes. Por defecto: 5 MB.</summary>
    public long TamanoMaximoBytes { get; set; } = 5 * 1024 * 1024;

    /// <summary>Tipos MIME aceptados para la subida.</summary>
    public string[] TiposPermitidos { get; set; } =
    [
        "image/jpeg",
        "image/png",
        "image/webp",
        "image/gif"
    ];
}
