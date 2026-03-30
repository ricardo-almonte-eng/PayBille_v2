using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PayBille.Api.Models.Catalogo;

/// <summary>Marcas globales para autocompletar.</summary>
public sealed class CatalogoMarca
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [BsonElement("imagen")]
    public string? Imagen { get; set; }

    [BsonElement("activa")]
    public bool Activa { get; set; } = true;

    [BsonElement("creadoEnUtc")]
    public DateTime CreadoEnUtc { get; set; } = DateTime.UtcNow;
}
