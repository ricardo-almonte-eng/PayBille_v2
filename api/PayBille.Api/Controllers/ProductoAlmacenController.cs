using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayBille.Api.DTOs;
using PayBille.Api.DTOs.Imagen;
using PayBille.Api.Interfaces;

namespace PayBille.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class ProductoAlmacenController : ControllerBase
{
    private readonly IProductoAlmacenService _productoAlmacenService;
    private readonly IImagenService          _imagenService;

    public ProductoAlmacenController(
        IProductoAlmacenService productoAlmacenService,
        IImagenService imagenService)
    {
        _productoAlmacenService = productoAlmacenService;
        _imagenService          = imagenService;
    }

    /// <summary>
    /// Sube y asocia una imagen al producto de almacén indicado.
    /// La imagen se puede cambiar independientemente del catálogo global.
    /// El archivo debe enviarse como multipart/form-data en el campo "archivo".
    /// </summary>
    [HttpPost("{id}/imagen")]
    [ProducesResponseType(typeof(ApiRespDto<ImagenResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SubirImagen(
        string id,
        IFormFile archivo,
        CancellationToken cancellationToken)
    {
        var resultImagen = await _imagenService.SubirAsync(archivo, "almacen-productos", cancellationToken);
        if (!resultImagen.IsSuccess)
            return Ok(ApiRespDto<ImagenResDto>.Error(resultImagen.Error!));

        var resultActualizar = await _productoAlmacenService.ActualizarImagenAsync(id, resultImagen.Value!.Url, cancellationToken);
        return Ok(resultActualizar.IsSuccess
            ? ApiRespDto<ImagenResDto>.Ok(resultImagen.Value!)
            : ApiRespDto<ImagenResDto>.Error(resultActualizar.Error!));
    }
}
