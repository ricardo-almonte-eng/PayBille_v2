using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayBille.Api.DTOs;
using PayBille.Api.DTOs.Auth;
using PayBille.Api.Interfaces;

namespace PayBille.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IJwtService _jwtService;

    public AuthController(IJwtService jwtService)
    {
        _jwtService = jwtService;
    }

    /// <summary>
    /// Autentica un usuario y retorna un par de tokens de acceso y refresco.
    /// Este endpoint es público (no requiere token).
    /// </summary>
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiRespDto<UserAuthResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login(
        [FromBody] UserLoginReqDto request,
        CancellationToken cancellationToken)
    {
        var result = await _jwtService.LoginAsync(request.Username, request.Password, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<UserAuthResDto>.Ok(result.Value!)
            : ApiRespDto<UserAuthResDto>.Error(result.Error!));
    }

    /// <summary>
    /// Intercambia un token de refresco válido por un nuevo par de tokens.
    /// Este endpoint es público (el token de acceso expirado no se usa aquí).
    /// </summary>
    [AllowAnonymous]
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(ApiRespDto<UserAuthResDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Refresh(
        [FromBody] UserRefreshTokenReqDto request,
        CancellationToken cancellationToken)
    {
        var result = await _jwtService.RefreshAsync(request.RefreshToken, cancellationToken);
        return Ok(result.IsSuccess
            ? ApiRespDto<UserAuthResDto>.Ok(result.Value!)
            : ApiRespDto<UserAuthResDto>.Error(result.Error!));
    }

    /// <summary>
    /// Revoca un token de refresco (cierre de sesión).
    /// Requiere token de acceso válido.
    /// </summary>
    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(typeof(ApiRespDto<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout(
        [FromBody] UserRefreshTokenReqDto request,
        CancellationToken cancellationToken)
    {
        await _jwtService.RevokeAsync(request.RefreshToken, cancellationToken);
        return Ok(ApiRespDto<bool>.Ok(true));
    }
}
