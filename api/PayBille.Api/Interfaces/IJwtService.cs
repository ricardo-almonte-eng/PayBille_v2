using PayBille.Api.Common;
using PayBille.Api.DTOs.Auth;
using PayBille.Api.Models;

namespace PayBille.Api.Interfaces;

public interface IJwtService
{
    /// <summary>Generates a signed JWT access token for the given persona.</summary>
    string GenerateAccessToken(Persona persona);

    /// <summary>Generates a cryptographically-random refresh token string.</summary>
    string GenerateRefreshToken();

    /// <summary>
    /// Validates credentials, issues a new access + refresh token pair.
    /// Returns Fail(AUL01) if credentials are invalid.
    /// </summary>
    Task<Result<UserAuthResDto>> LoginAsync(string username, string password, CancellationToken ct = default);

    /// <summary>
    /// Validates the supplied refresh token, revokes it, issues a new pair.
    /// Returns Fail(AUR01) if the token is invalid or expired.
    /// </summary>
    Task<Result<UserAuthResDto>> RefreshAsync(string refreshToken, CancellationToken ct = default);

    /// <summary>Revokes a refresh token so it can no longer be used.</summary>
    Task RevokeAsync(string refreshToken, CancellationToken ct = default);
}
