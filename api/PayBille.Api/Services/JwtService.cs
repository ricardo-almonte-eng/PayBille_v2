using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using PayBille.Api.Configuration;
using PayBille.Api.DTOs.Auth;
using PayBille.Api.Infrastructure;
using PayBille.Api.Models;

namespace PayBille.Api.Services;

public sealed class JwtService : IJwtService
{
    private readonly JwtSettings _jwt;
    private readonly IMongoCollection<User> _users;
    private readonly ILogger<JwtService> _logger;

    public JwtService(
        IOptions<JwtSettings> jwtOptions,
        MongoDbContext dbContext,
        ILogger<JwtService> logger)
    {
        _jwt = jwtOptions.Value;
        _users = dbContext.Database.GetCollection<User>("users");
        _logger = logger;
    }

    // -------------------------------------------------------------------------
    // Token generation
    // -------------------------------------------------------------------------

    /// <inheritdoc/>
    public string GenerateAccessToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.UtcNow.AddMinutes(_jwt.AccessTokenExpiryMinutes);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
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
    public async Task<AuthResponse> LoginAsync(string username, string password, CancellationToken ct = default)
    {
        var user = await _users
            .Find(u => u.Username == username)
            .FirstOrDefaultAsync(ct);

        if (user is null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid username or password.");

        return await BuildAndPersistTokenPairAsync(user, ct);
    }

    /// <inheritdoc/>
    public async Task<AuthResponse> RefreshAsync(string refreshToken, CancellationToken ct = default)
    {
        var user = await _users
            .Find(u => u.RefreshTokens.Any(rt => rt.Token == refreshToken))
            .FirstOrDefaultAsync(ct);

        if (user is null)
            throw new UnauthorizedAccessException("Invalid refresh token.");

        var storedToken = user.RefreshTokens.First(rt => rt.Token == refreshToken);

        if (!storedToken.IsActive)
            throw new UnauthorizedAccessException("Refresh token is no longer active.");

        // Revoke the old token
        storedToken.RevokedAtUtc = DateTime.UtcNow;

        return await BuildAndPersistTokenPairAsync(user, ct);
    }

    /// <inheritdoc/>
    public async Task RevokeAsync(string refreshToken, CancellationToken ct = default)
    {
        var user = await _users
            .Find(u => u.RefreshTokens.Any(rt => rt.Token == refreshToken))
            .FirstOrDefaultAsync(ct);

        if (user is null)
            return;

        var storedToken = user.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshToken);
        if (storedToken is null || !storedToken.IsActive)
            return;

        storedToken.RevokedAtUtc = DateTime.UtcNow;

        await PersistUserAsync(user, ct);
        _logger.LogInformation("Refresh token revoked for user {UserId}.", user.Id);
    }

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    private async Task<AuthResponse> BuildAndPersistTokenPairAsync(User user, CancellationToken ct)
    {
        var expiry = DateTime.UtcNow.AddMinutes(_jwt.AccessTokenExpiryMinutes);
        var accessToken = GenerateAccessToken(user);
        var rawRefresh = GenerateRefreshToken();

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = rawRefresh,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(_jwt.RefreshTokenExpiryDays),
            CreatedAtUtc = DateTime.UtcNow
        });

        // Keep only the last 5 refresh tokens per user to avoid unbounded growth
        user.RefreshTokens = user.RefreshTokens
            .OrderByDescending(rt => rt.CreatedAtUtc)
            .Take(5)
            .ToList();

        await PersistUserAsync(user, ct);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = rawRefresh,
            ExpiresAtUtc = expiry
        };
    }

    private async Task PersistUserAsync(User user, CancellationToken ct)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
        var update = Builders<User>.Update.Set(u => u.RefreshTokens, user.RefreshTokens);
        await _users.UpdateOneAsync(filter, update, cancellationToken: ct);
    }
}
