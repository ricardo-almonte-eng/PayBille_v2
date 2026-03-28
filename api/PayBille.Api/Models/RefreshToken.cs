using MongoDB.Bson.Serialization.Attributes;

namespace PayBille.Api.Models;

public sealed class RefreshToken
{
    [BsonElement("token")]
    public string Token { get; set; } = string.Empty;

    [BsonElement("expiresAtUtc")]
    public DateTime ExpiresAtUtc { get; set; }

    [BsonElement("createdAtUtc")]
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    [BsonElement("revokedAtUtc")]
    public DateTime? RevokedAtUtc { get; set; }

    [BsonIgnore]
    public bool IsExpired => DateTime.UtcNow >= ExpiresAtUtc;

    [BsonIgnore]
    public bool IsRevoked => RevokedAtUtc.HasValue;

    [BsonIgnore]
    public bool IsActive => !IsRevoked && !IsExpired;
}
