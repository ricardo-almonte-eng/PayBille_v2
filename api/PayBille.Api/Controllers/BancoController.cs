using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayBille.Api.DTOs;
using PayBille.Api.DTOs.Banco;
using PayBille.Api.Errors;
using PayBille.Api.Interfaces;

namespace PayBille.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class BancoController : ControllerBase
{
    private readonly IBancoService          _bancoService;
    private readonly IValidator<BancoReqDto> _validator;

    public BancoController(IBancoService bancoService, IValidator<BancoReqDto> validator)
    {
        _bancoService = bancoService;
        _validator    = validator;
    }

    /// <summary>
    /// Obtiene la lista completa de bancos del catálogo.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiRespDto<List<BancoResDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerTodos(CancellationToken cancellationToken)
    {
        var result = await _bancoService.ObtenerTodosAsync(cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<List<BancoResDto>>.Ok(result.Value!)
            : ApiRespDto<List<BancoResDto>>.Error(result.Error!));
    }

    /// <summary>
    /// Obtiene un banco por su identificador único (IdBanco).
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiRespDto<BancoResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerPorId(string id, CancellationToken cancellationToken)
    {
        var result = await _bancoService.ObtenerPorIdAsync(id, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<BancoResDto>.Ok(result.Value!)
            : ApiRespDto<BancoResDto>.Error(result.Error!));
    }

    /// <summary>
    /// Registra un nuevo banco en el catálogo. El IdBanco (GUID) se genera automáticamente en el servidor.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiRespDto<BancoResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Crear(
        [FromBody] BancoReqDto request,
        CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            var detalle = string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage));
            return Ok(ApiRespDto<BancoResDto>.Error(AppErrors.BancoValidacionFallida(detalle)));
        }

        var result = await _bancoService.CrearAsync(request, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<BancoResDto>.Ok(result.Value!)
            : ApiRespDto<BancoResDto>.Error(result.Error!));
    }

    /// <summary>
    /// Actualiza los datos de un banco existente por su IdBanco (GUID).
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiRespDto<BancoResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Actualizar(
        string id,
        [FromBody] BancoReqDto request,
        CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            var detalle = string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage));
            return Ok(ApiRespDto<BancoResDto>.Error(AppErrors.BancoValidacionFallida(detalle)));
        }

        var result = await _bancoService.ActualizarAsync(id, request, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<BancoResDto>.Ok(result.Value!)
            : ApiRespDto<BancoResDto>.Error(result.Error!));
    }

    /// <summary>
    /// Elimina un banco del catálogo por su identificador único (IdBanco).
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiRespDto<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Eliminar(string id, CancellationToken cancellationToken)
    {
        var result = await _bancoService.EliminarAsync(id, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<bool>.Ok(true)
            : ApiRespDto<bool>.Error(result.Error!));
    }
}
