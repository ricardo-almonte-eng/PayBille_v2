using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using PayBille.Api.Common;
using PayBille.Api.Configuration;
using PayBille.Api.DTOs.Auth;
using PayBille.Api.Errors;
using PayBille.Api.Infrastructure.Repositories;
using PayBille.Api.Interfaces;
using PayBille.Api.Models;

namespace PayBille.Api.Services;

public sealed class JwtService : IJwtService
{
    private readonly JwtSettings _jwt;
    private readonly PersonaRepository _personaRepository;
    private readonly ILogger<JwtService> _logger;

    public JwtService(
        IOptions<JwtSettings> jwtOptions,
        PersonaRepository personaRepository,
        ILogger<JwtService> logger)
    {
        _jwt = jwtOptions.Value;
        _personaRepository = personaRepository;
        _logger = logger;
    }

    // -------------------------------------------------------------------------
    // Token generation
    // -------------------------------------------------------------------------

    /// <inheritdoc/>
    public string GenerateAccessToken(Persona persona)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.UtcNow.AddMinutes(_jwt.AccessTokenExpiryMinutes);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, persona.Id),
            new Claim(JwtRegisteredClaimNames.UniqueName, persona.Usuario.NombreUsuario),
            new Claim(ClaimTypes.Role, persona.Usuario.IdRol.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: expiry,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <inheritdoc/>
    public string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }

    // -------------------------------------------------------------------------
    // Auth operations
    // -------------------------------------------------------------------------

    /// <inheritdoc/>
    public async Task<Result<UserAuthResDto>> LoginAsync(string username, string password, CancellationToken ct = default)
    {
        var persona = await _personaRepository.FindByNombreUsuarioAsync(username, ct);

        if (persona is null || !BCrypt.Net.BCrypt.Verify(password, persona.Usuario.ContrasenaHash))
            return Result<UserAuthResDto>.Fail(AppErrors.AuthCredencialesInvalidas());

        return Result<UserAuthResDto>.Ok(await BuildAndPersistTokenPairAsync(persona, ct));
    }

    /// <inheritdoc/>
    public async Task<Result<UserAuthResDto>> RefreshAsync(string refreshToken, CancellationToken ct = default)
    {
        var filter = Builders<Persona>.Filter.ElemMatch(p => p.Usuario.TokensRefresh, rt => rt.Token == refreshToken);
        var persona = await _personaRepository.FindOneAsync(filter, ct);

        if (persona is null)
            return Result<UserAuthResDto>.Fail(AppErrors.AuthTokenInvalido());

        var storedToken = persona.Usuario.TokensRefresh.First(rt => rt.Token == refreshToken);

        if (!storedToken.IsActive)
            return Result<UserAuthResDto>.Fail(AppErrors.AuthTokenInvalido());

        // Revoke the old token
        storedToken.RevokedAtUtc = DateTime.UtcNow;

        return Result<UserAuthResDto>.Ok(await BuildAndPersistTokenPairAsync(persona, ct));
    }

    /// <inheritdoc/>
    public async Task RevokeAsync(string refreshToken, CancellationToken ct = default)
    {
        var filter = Builders<Persona>.Filter.ElemMatch(p => p.Usuario.TokensRefresh, rt => rt.Token == refreshToken);
        var persona = await _personaRepository.FindOneAsync(filter, ct);

        if (persona is null)
            return;

        var storedToken = persona.Usuario.TokensRefresh.FirstOrDefault(rt => rt.Token == refreshToken);
        if (storedToken is null || !storedToken.IsActive)
            return;

        storedToken.RevokedAtUtc = DateTime.UtcNow;

        await PersistPersonaAsync(persona, ct);
        _logger.LogInformation("Refresh token revoked for persona {PersonaId}.", persona.Id);
    }

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    private async Task<UserAuthResDto> BuildAndPersistTokenPairAsync(Persona persona, CancellationToken ct)
    {
        var expiry = DateTime.UtcNow.AddMinutes(_jwt.AccessTokenExpiryMinutes);
        var accessToken = GenerateAccessToken(persona);
        var rawRefresh = GenerateRefreshToken();

        persona.Usuario.TokensRefresh.Add(new RefreshToken
        {
            Token = rawRefresh,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(_jwt.RefreshTokenExpiryDays),
            CreatedAtUtc = DateTime.UtcNow
        });

        // Keep only the last 5 refresh tokens per persona to avoid unbounded growth
        persona.Usuario.TokensRefresh = persona.Usuario.TokensRefresh
            .OrderByDescending(rt => rt.CreatedAtUtc)
            .Take(5)
            .ToList();

        await PersistPersonaAsync(persona, ct);

        return new UserAuthResDto
        {
            AccessToken = accessToken,
            RefreshToken = rawRefresh,
            ExpiresAtUtc = expiry
        };
    }

    private async Task PersistPersonaAsync(Persona persona, CancellationToken ct)
    {
        var filter = Builders<Persona>.Filter.Eq(p => p.Id, persona.Id);
        var update = Builders<Persona>.Update.Set(p => p.Usuario.TokensRefresh, persona.Usuario.TokensRefresh);
        await _personaRepository.UpdateOneAsync(filter, update, ct);
    }
}
