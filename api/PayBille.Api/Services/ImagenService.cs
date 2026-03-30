using Microsoft.Extensions.Options;
using PayBille.Api.Common;
using PayBille.Api.Configuration;
using PayBille.Api.DTOs.Imagen;
using PayBille.Api.Errors;
using PayBille.Api.Interfaces;

namespace PayBille.Api.Services;

/// <summary>
/// Servicio reutilizable para guardar imágenes en el sistema de archivos local.
/// El directorio base se configura a través de <see cref="ImagenSettings"/>.
/// </summary>
public sealed class ImagenService : IImagenService
{
    private readonly ImagenSettings _settings;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<ImagenService> _logger;

    public ImagenService(
        IOptions<ImagenSettings> settings,
        IWebHostEnvironment env,
        ILogger<ImagenService> logger)
    {
        _settings = settings.Value;
        _env      = env;
        _logger   = logger;
    }

    /// <inheritdoc/>
    public async Task<Result<ImagenResDto>> SubirAsync(
        IFormFile archivo,
        string subDirectorio,
        CancellationToken ct)
    {
        // Validar que el archivo no esté vacío
        if (archivo is null || archivo.Length == 0)
            return Result<ImagenResDto>.Fail(AppErrors.ImagenArchivoRequerido());

        // Validar tipo MIME
        if (!_settings.TiposPermitidos.Contains(archivo.ContentType, StringComparer.OrdinalIgnoreCase))
            return Result<ImagenResDto>.Fail(AppErrors.ImagenTipoNoPermitido(archivo.ContentType));

        // Validar tamaño
        if (archivo.Length > _settings.TamanoMaximoBytes)
            return Result<ImagenResDto>.Fail(AppErrors.ImagenTamanoExcedido(_settings.TamanoMaximoBytes));

        // Construir ruta absoluta de destino
        var baseDir = Path.IsPathRooted(_settings.DirectorioBase)
            ? _settings.DirectorioBase
            : Path.Combine(_env.ContentRootPath, _settings.DirectorioBase);

        var directorioDestino = Path.Combine(baseDir, subDirectorio);
        Directory.CreateDirectory(directorioDestino);

        // Generar nombre único para evitar colisiones
        var extension     = Path.GetExtension(archivo.FileName);
        var nombreArchivo = $"{Guid.NewGuid()}{extension}";
        var rutaCompleta  = Path.Combine(directorioDestino, nombreArchivo);

        // Guardar el archivo en disco
        try
        {
            await using var stream = new FileStream(rutaCompleta, FileMode.Create, FileAccess.Write);
            await archivo.CopyToAsync(stream, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar la imagen en {Ruta}.", rutaCompleta);
            return Result<ImagenResDto>.Fail(AppErrors.ImagenGuardarErrorInterno());
        }

        // URL relativa expuesta por el middleware de archivos estáticos: /imagenes/{subDirectorio}/{nombre}
        var urlRelativa = $"imagenes/{subDirectorio}/{nombreArchivo}";

        _logger.LogInformation("Imagen guardada: {Url}", urlRelativa);

        return Result<ImagenResDto>.Ok(new ImagenResDto
        {
            Url           = urlRelativa,
            NombreArchivo = nombreArchivo,
        });
    }
}
