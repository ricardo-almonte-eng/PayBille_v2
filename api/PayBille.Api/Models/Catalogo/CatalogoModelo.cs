using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PayBille.Api.Models.Catalogo;

/// <summary>Modelos globales vinculados a una marca para autocompletar.</summary>
public sealed class CatalogoModelo
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [BsonElement("idMarca")]
    public string? IdMarca { get; set; }

    /// <summary>Nombre de la marca desnormalizado para búsquedas sin join.</summary>
    [BsonElement("marca")]
    public string? Marca { get; set; }

    [BsonElement("activo")]
    public bool Activo { get; set; } = true;

    [BsonElement("creadoEnUtc")]
    public DateTime CreadoEnUtc { get; set; } = DateTime.UtcNow;
}
