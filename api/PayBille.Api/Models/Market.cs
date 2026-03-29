using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PayBille.Api.Models;

public sealed class Market
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("idMarket")]
    [BsonRequired]
    public string IdMarket { get; set; } = string.Empty;

    [BsonElement("name")]
    [BsonRequired]
    public string Name { get; set; } = string.Empty;

    [BsonElement("active")]
    public bool Active { get; set; }

    [BsonElement("mail")]
    public string? Mail { get; set; }

    [BsonElement("description")]
    public string? Description { get; set; }

    [BsonElement("circularLink")]
    public string? CircularLink { get; set; }

    [BsonElement("address")]
    [BsonRequired]
    public string Address { get; set; } = string.Empty;

    [BsonElement("address2")]
    public string? Address2 { get; set; }

    [BsonElement("phone")]
    public string? Phone { get; set; }

    [BsonElement("phone2")]
    public string? Phone2 { get; set; }

    [BsonElement("idOwner")]
    [BsonRequired]
    public string IdOwner { get; set; } = string.Empty;

    [BsonElement("bank")]
    public string? Bank { get; set; }

    [BsonElement("image")]
    public string? Image { get; set; }

    [BsonElement("rnc")]
    public string? RNC { get; set; }

    [BsonElement("taxValue")]
    public decimal TaxValue { get; set; } = 0;

    [BsonElement("creadoEnUtc")]
    public DateTime CreadoEnUtc { get; set; } = DateTime.UtcNow;
}
