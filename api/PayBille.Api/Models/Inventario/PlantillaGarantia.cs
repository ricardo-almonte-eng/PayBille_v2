using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PayBille.Api.Models.Inventario;

/// <summary>
/// Plantilla de garantía configurable por empresa.
/// Al asignar un producto a una sucursal, se hace snapshot del texto
/// para que el historial no cambie si la plantilla es editada en el futuro.
/// </summary>
public sealed class PlantillaGarantia
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("idEmpresa")]
    public string IdEmpresa { get; set; } = string.Empty;

    [BsonElement("nombre")]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Texto/HTML completo del contrato de garantía.</summary>
    [BsonElement("descripcion")]
    public string? Descripcion { get; set; }

    [BsonElement("duracionDiasPorDefecto")]
    public int DuracionDiasPorDefecto { get; set; } = 30;

    [BsonElement("activa")]
    public bool Activa { get; set; } = true;

    [BsonElement("creadoEnUtc")]
    public DateTime CreadoEnUtc { get; set; } = DateTime.UtcNow;
}
