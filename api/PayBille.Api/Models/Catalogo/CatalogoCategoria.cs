using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PayBille.Api.Models.Catalogo;

/// <summary>Categorías globales con soporte para subcategorías.</summary>
public sealed class CatalogoCategoria
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [BsonElement("idCategoriaPadre")]
    public string? IdCategoriaPadre { get; set; }

    [BsonElement("icono")]
    public string? Icono { get; set; }

    [BsonElement("activa")]
    public bool Activa { get; set; } = true;

    [BsonElement("creadoEnUtc")]
    public DateTime CreadoEnUtc { get; set; } = DateTime.UtcNow;
}
