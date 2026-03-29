// ── PayBille v2 Controller Template ─────────────────────────────────────────
// Reglas:
//   - Todos los endpoints retornan HTTP 200 (errors de negocio en body)
//   - Sin try/catch — los errores de dominio los maneja Result<T>
//   - FluentValidation llamado manualmente antes del servicio
//   - Usar ApiRespDto<T>.Ok() / ApiRespDto<T>.Error(result.Error!)
// ─────────────────────────────────────────────────────────────────────────────
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayBille.Api.DTOs;
using PayBille.Api.DTOs.{Entidad};
using PayBille.Api.Errors;
using PayBille.Api.Interfaces;

namespace PayBille.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class {Entidad}Controller : ControllerBase
{
    private readonly I{Entidad}Service _{entidad}Service;
    private readonly IValidator<{Entidad}ReqDto> _validator;

    public {Entidad}Controller(I{Entidad}Service {entidad}Service, IValidator<{Entidad}ReqDto> validator)
    {
        _{entidad}Service = {entidad}Service;
        _validator        = validator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiRespDto<List<{Entidad}ResDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerTodos(CancellationToken cancellationToken)
    {
        var result = await _{entidad}Service.ObtenerTodosAsync(cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<List<{Entidad}ResDto>>.Ok(result.Value!)
            : ApiRespDto<List<{Entidad}ResDto>>.Error(result.Error!));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiRespDto<{Entidad}ResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerPorId(string id, CancellationToken cancellationToken)
    {
        var result = await _{entidad}Service.ObtenerPorIdAsync(id, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<{Entidad}ResDto>.Ok(result.Value!)
            : ApiRespDto<{Entidad}ResDto>.Error(result.Error!));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiRespDto<{Entidad}ResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Crear([FromBody] {Entidad}ReqDto request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            var detalle = string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage));
            return Ok(ApiRespDto<{Entidad}ResDto>.Error(AppErrors.{Entidad}ValidacionFallida(detalle)));
        }

        var result = await _{entidad}Service.CrearAsync(request, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<{Entidad}ResDto>.Ok(result.Value!)
            : ApiRespDto<{Entidad}ResDto>.Error(result.Error!));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiRespDto<{Entidad}ResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Actualizar(string id, [FromBody] {Entidad}ReqDto request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            var detalle = string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage));
            return Ok(ApiRespDto<{Entidad}ResDto>.Error(AppErrors.{Entidad}ValidacionFallida(detalle)));
        }

        var result = await _{entidad}Service.ActualizarAsync(id, request, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<{Entidad}ResDto>.Ok(result.Value!)
            : ApiRespDto<{Entidad}ResDto>.Error(result.Error!));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiRespDto<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Eliminar(string id, CancellationToken cancellationToken)
    {
        var result = await _{entidad}Service.EliminarAsync(id, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<bool>.Ok(true)
            : ApiRespDto<bool>.Error(result.Error!));
    }
}
