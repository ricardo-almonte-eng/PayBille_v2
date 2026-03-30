using MongoDB.Bson.Serialization.Attributes;

namespace PayBille.Api.Models.Inventario.Embedded;

/// <summary>
/// Garantía aplicada al item del inventario de sucursal.
/// Hace snapshot del texto de la plantilla para preservar el historial
/// incluso si la plantilla es editada o eliminada.
/// </summary>
public sealed class GarantiaAplicada
{
    [BsonElement("idPlantilla")]
    public string IdPlantilla { get; set; } = string.Empty;

    [BsonElement("nombrePlantilla")]
    public string NombrePlantilla { get; set; } = string.Empty;

    [BsonElement("duracionDias")]
    public int DuracionDias { get; set; }

    /// <summary>Copia del texto de garantía en el momento de asignar — inmutable.</summary>
    [BsonElement("textoSnapshot")]
    public string? TextoSnapshot { get; set; }
}
