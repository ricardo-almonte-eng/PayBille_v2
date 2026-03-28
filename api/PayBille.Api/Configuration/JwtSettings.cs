namespace PayBille.Api.Configuration;

public sealed class JwtSettings
{
    public const string SectionName = "Jwt";

    /// <summary>Secret key used to sign JWT tokens (at least 32 characters).</summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>Token issuer (e.g. "paybille-api").</summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>Token audience (e.g. "paybille-client").</summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>Access-token lifetime in minutes. Default: 15.</summary>
    public int AccessTokenExpiryMinutes { get; set; } = 15;

    /// <summary>Refresh-token lifetime in days. Default: 7.</summary>
    public int RefreshTokenExpiryDays { get; set; } = 7;
}
