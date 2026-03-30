using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PayBille.Api.Models.Catalogo;

/// <summary>
/// Catálogo global de productos compartido entre todas las empresas.
/// Solo para autocompletar al registrar un producto nuevo en el almacén.
/// Ninguna empresa es propietaria de este documento.
/// </summary>
public sealed class CatalogoProducto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [BsonElement("codigoBarra")]
    public string? CodigoBarra { get; set; }

    [BsonElement("marca")]
    public string? Marca { get; set; }

    [BsonElement("modelo")]
    public string? Modelo { get; set; }

    [BsonElement("categoria")]
    public string? Categoria { get; set; }

    [BsonElement("color")]
    public string? Color { get; set; }

    [BsonElement("imagen")]
    public string? Imagen { get; set; }

    [BsonElement("descripcion")]
    public string? Descripcion { get; set; }

    [BsonElement("unidadMedida")]
    public string? UnidadMedida { get; set; }

    [BsonElement("creadoEnUtc")]
    public DateTime CreadoEnUtc { get; set; } = DateTime.UtcNow;
}
