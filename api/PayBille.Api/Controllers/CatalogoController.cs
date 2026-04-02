using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayBille.Api.Common;
using PayBille.Api.DTOs;
using PayBille.Api.DTOs.Catalogo;

namespace PayBille.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class CatalogoController : ControllerBase
{
    /// <summary>
    /// Retorna la lista de zonas horarias disponibles para llenar un select.
    /// Cada elemento incluye el valor del enum, la etiqueta legible, el identificador IANA
    /// y el desplazamiento UTC.
    /// </summary>
    [HttpGet("zonas-horarias")]
    [ProducesResponseType(typeof(ApiRespDto<List<ZonaHorariaItemDto>>), StatusCodes.Status200OK)]
    public IActionResult ObtenerZonasHorarias()
    {
        var lista = ZonaHorariaExtensions.ObtenerTodasComoLista();
        return Ok(ApiRespDto<List<ZonaHorariaItemDto>>.Ok(lista));
    }
}
