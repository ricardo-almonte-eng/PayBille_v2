using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayBille.Api.DTOs;
using PayBille.Api.DTOs.Imagen;
using PayBille.Api.DTOs.Persona;
using PayBille.Api.Errors;
using PayBille.Api.Interfaces;

namespace PayBille.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class PersonaController : ControllerBase
{
    private readonly IPersonaService _personaService;
    private readonly IImagenService _imagenService;
    private readonly IValidator<PersonaReqDto> _validator;

    public PersonaController(
        IPersonaService personaService,
        IImagenService imagenService,
        IValidator<PersonaReqDto> validator)
    {
        _personaService = personaService;
        _imagenService  = imagenService;
        _validator      = validator;
    }

    /// <summary>
    /// Obtiene la lista completa de personas registradas.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiRespDto<List<PersonaResDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerTodos(CancellationToken cancellationToken)
    {
        var result = await _personaService.ObtenerTodosAsync(cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<List<PersonaResDto>>.Ok(result.Value!)
            : ApiRespDto<List<PersonaResDto>>.Error(result.Error!));
    }

    /// <summary>
    /// Obtiene una persona por su identificador único (IdPersona).
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiRespDto<PersonaResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerPorId(string id, CancellationToken cancellationToken)
    {
        var result = await _personaService.ObtenerPorIdAsync(id, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<PersonaResDto>.Ok(result.Value!)
            : ApiRespDto<PersonaResDto>.Error(result.Error!));
    }

    /// <summary>
    /// Registra una nueva persona. El IdPersona (GUID) se genera automáticamente en el servidor.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiRespDto<PersonaResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Crear(
        [FromBody] PersonaReqDto request,
        CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            var detalle = string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage));
            return Ok(ApiRespDto<PersonaResDto>.Error(AppErrors.PersonaValidacionFallida(detalle)));
        }

        var result = await _personaService.CrearOActualizarAsync(null, request, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<PersonaResDto>.Ok(result.Value!)
            : ApiRespDto<PersonaResDto>.Error(result.Error!));
    }

    /// <summary>
    /// Actualiza los datos de una persona existente por su IdPersona (GUID).
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiRespDto<PersonaResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Actualizar(
        string id,
        [FromBody] PersonaReqDto request,
        CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            var detalle = string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage));
            return Ok(ApiRespDto<PersonaResDto>.Error(AppErrors.PersonaValidacionFallida(detalle)));
        }

        var result = await _personaService.CrearOActualizarAsync(id, request, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<PersonaResDto>.Ok(result.Value!)
            : ApiRespDto<PersonaResDto>.Error(result.Error!));
    }

    /// <summary>
    /// Elimina una persona por su identificador único (IdPersona).
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiRespDto<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Eliminar(string id, CancellationToken cancellationToken)
    {
        var result = await _personaService.EliminarAsync(id, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<bool>.Ok(true)
            : ApiRespDto<bool>.Error(result.Error!));
    }

    /// <summary>
    /// Sube y asocia una imagen de perfil a la persona indicada por IdPersona.
    /// El archivo debe enviarse como multipart/form-data en el campo "archivo".
    /// </summary>
    [HttpPost("{id}/imagen")]
    [ProducesResponseType(typeof(ApiRespDto<ImagenResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SubirImagen(
        string id,
        IFormFile archivo,
        CancellationToken cancellationToken)
    {
        var resultImagen = await _imagenService.SubirAsync(archivo, "personas", cancellationToken);
        if (!resultImagen.IsSuccess)
            return Ok(ApiRespDto<ImagenResDto>.Error(resultImagen.Error!));

        var resultActualizar = await _personaService.ActualizarImagenAsync(id, resultImagen.Value!.Url, cancellationToken);
        return Ok(resultActualizar.IsSuccess
            ? ApiRespDto<ImagenResDto>.Ok(resultImagen.Value!)
            : ApiRespDto<ImagenResDto>.Error(resultActualizar.Error!));
    }
}
