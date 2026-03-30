using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PayBille.Api.Models.Catalogo;

/// <summary>Colores globales con valor HEX para UI.</summary>
public sealed class CatalogoColor
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("nombre")]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Valor hexadecimal del color, ej: "#1C1C1E". Para mostrar chip de color en la UI.</summary>
    [BsonElement("hex")]
    public string? Hex { get; set; }

    [BsonElement("activo")]
    public bool Activo { get; set; } = true;
}
