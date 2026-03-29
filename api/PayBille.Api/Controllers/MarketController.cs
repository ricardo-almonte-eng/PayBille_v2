using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayBille.Api.DTOs;
using PayBille.Api.DTOs.Imagen;
using PayBille.Api.DTOs.Market;
using PayBille.Api.Errors;
using PayBille.Api.Interfaces;

namespace PayBille.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class MarketController : ControllerBase
{
    private readonly IMarketService _marketService;
    private readonly IImagenService _imagenService;
    private readonly IValidator<MarketReqDto> _validator;

    public MarketController(
        IMarketService marketService,
        IImagenService imagenService,
        IValidator<MarketReqDto> validator)
    {
        _marketService = marketService;
        _imagenService = imagenService;
        _validator     = validator;
    }

    /// <summary>
    /// Obtiene la lista completa de markets registrados.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiRespDto<List<MarketResDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerTodos(CancellationToken cancellationToken)
    {
        var result = await _marketService.ObtenerTodosAsync(cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<List<MarketResDto>>.Ok(result.Value!)
            : ApiRespDto<List<MarketResDto>>.Error(result.Error!));
    }

    /// <summary>
    /// Obtiene un market por su identificador único (IdMarket).
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiRespDto<MarketResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerPorId(string id, CancellationToken cancellationToken)
    {
        var result = await _marketService.ObtenerPorIdAsync(id, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<MarketResDto>.Ok(result.Value!)
            : ApiRespDto<MarketResDto>.Error(result.Error!));
    }

    /// <summary>
    /// Registra un nuevo market. El IdMarket (GUID) se genera automáticamente en el servidor.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiRespDto<MarketResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Crear(
        [FromBody] MarketReqDto request,
        CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            var detalle = string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage));
            return Ok(ApiRespDto<MarketResDto>.Error(AppErrors.MarketValidacionFallida(detalle)));
        }

        var result = await _marketService.CrearOActualizarAsync(null, request, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<MarketResDto>.Ok(result.Value!)
            : ApiRespDto<MarketResDto>.Error(result.Error!));
    }

    /// <summary>
    /// Actualiza los datos de un market existente por su IdMarket (GUID).
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiRespDto<MarketResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Actualizar(
        string id,
        [FromBody] MarketReqDto request,
        CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            var detalle = string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage));
            return Ok(ApiRespDto<MarketResDto>.Error(AppErrors.MarketValidacionFallida(detalle)));
        }

        var result = await _marketService.CrearOActualizarAsync(id, request, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<MarketResDto>.Ok(result.Value!)
            : ApiRespDto<MarketResDto>.Error(result.Error!));
    }

    /// <summary>
    /// Elimina un market por su identificador único (IdMarket).
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiRespDto<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Eliminar(string id, CancellationToken cancellationToken)
    {
        var result = await _marketService.EliminarAsync(id, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<bool>.Ok(true)
            : ApiRespDto<bool>.Error(result.Error!));
    }

    /// <summary>
    /// Sube y asocia una imagen al market indicado por IdMarket.
    /// El archivo debe enviarse como multipart/form-data en el campo "archivo".
    /// </summary>
    [HttpPost("{id}/imagen")]
    [ProducesResponseType(typeof(ApiRespDto<ImagenResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SubirImagen(
        string id,
        IFormFile archivo,
        CancellationToken cancellationToken)
    {
        var resultImagen = await _imagenService.SubirAsync(archivo, "markets", cancellationToken);
        if (!resultImagen.IsSuccess)
            return Ok(ApiRespDto<ImagenResDto>.Error(resultImagen.Error!));

        var resultActualizar = await _marketService.ActualizarImagenAsync(id, resultImagen.Value!.Url, cancellationToken);
        return Ok(resultActualizar.IsSuccess
            ? ApiRespDto<ImagenResDto>.Ok(resultImagen.Value!)
            : ApiRespDto<ImagenResDto>.Error(resultActualizar.Error!));
    }
}
