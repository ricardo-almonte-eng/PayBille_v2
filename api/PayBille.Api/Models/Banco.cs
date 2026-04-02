using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PayBille.Api.Models;

/// <summary>
/// Catálogo de bancos disponibles para asociar a una cuenta bancaria.
/// Permite que los usuarios seleccionen el banco sin escribirlo manualmente.
/// </summary>
public sealed class Banco
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("idBanco")]
    [BsonRequired]
    public string IdBanco { get; set; } = string.Empty;

    [BsonElement("nombre")]
    [BsonRequired]
    public string Nombre { get; set; } = string.Empty;

    [BsonElement("codigo")]
    public string? Codigo { get; set; }

    [BsonElement("activo")]
    public bool Activo { get; set; } = true;

    [BsonElement("creadoEnUtc")]
    public DateTime CreadoEnUtc { get; set; } = DateTime.UtcNow;
}
