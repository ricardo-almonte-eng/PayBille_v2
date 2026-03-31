using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayBille.Api.DTOs;
using PayBille.Api.DTOs.Imagen;
using PayBille.Api.Interfaces;

namespace PayBille.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class CatalogoProductoController : ControllerBase
{
    private readonly ICatalogoProductoService _catalogoProductoService;
    private readonly IImagenService           _imagenService;

    public CatalogoProductoController(
        ICatalogoProductoService catalogoProductoService,
        IImagenService imagenService)
    {
        _catalogoProductoService = catalogoProductoService;
        _imagenService           = imagenService;
    }

    /// <summary>
    /// Sube y asocia una imagen al producto de catálogo indicado.
    /// El archivo debe enviarse como multipart/form-data en el campo "archivo".
    /// </summary>
    [HttpPost("{id}/imagen")]
    [ProducesResponseType(typeof(ApiRespDto<ImagenResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SubirImagen(
        string id,
        IFormFile archivo,
        CancellationToken cancellationToken)
    {
        var resultImagen = await _imagenService.SubirAsync(archivo, "catalogo-productos", cancellationToken);
        if (!resultImagen.IsSuccess)
            return Ok(ApiRespDto<ImagenResDto>.Error(resultImagen.Error!));

        var resultActualizar = await _catalogoProductoService.ActualizarImagenAsync(id, resultImagen.Value!.Url, cancellationToken);
        return Ok(resultActualizar.IsSuccess
            ? ApiRespDto<ImagenResDto>.Ok(resultImagen.Value!)
            : ApiRespDto<ImagenResDto>.Error(resultActualizar.Error!));
    }
}
