using PayBille.Api.DTOs.Auth;
using PayBille.Api.Models;

namespace PayBille.Api.Interfaces;

public interface IJwtService
{
    /// <summary>Generates a signed JWT access token for the given user.</summary>
    string GenerateAccessToken(User user);

    /// <summary>Generates a cryptographically-random refresh token string.</summary>
    string GenerateRefreshToken();

    /// <summary>
    /// Validates credentials, issues a new access + refresh token pair,
    /// and persists the refresh token in the user document.
    /// </summary>
    Task<UserAuthResDto> LoginAsync(string username, string password, CancellationToken ct = default);

    /// <summary>
    /// Validates the supplied refresh token, revokes it, issues a new pair,
    /// and persists the new refresh token.
    /// </summary>
    Task<UserAuthResDto> RefreshAsync(string refreshToken, CancellationToken ct = default);

    /// <summary>Revokes a refresh token so it can no longer be used.</summary>
    Task RevokeAsync(string refreshToken, CancellationToken ct = default);
}
