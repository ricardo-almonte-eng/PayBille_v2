using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PayBille.Api.Models.Inventario;

/// <summary>
/// Producto registrado en el almacén de la empresa.
/// Puede originarse de un autocompletado del CatalogoProducto o crearse manualmente.
/// No contiene stock ni precios — eso se define al asignarlo al inventario de una sucursal.
/// </summary>
public sealed class ProductoAlmacen
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    /// <summary>
    /// Referencia al catálogo global si fue autocompletado. Null si fue creado manualmente.
    /// </summary>
    [BsonElement("idCatalogo")]
    public string? IdCatalogo { get; set; }

    [BsonElement("idEmpresa")]
    public string IdEmpresa { get; set; } = string.Empty;

    [BsonElement("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [BsonElement("descripcion")]
    public string? Descripcion { get; set; }

    [BsonElement("codigoBarra")]
    public string? CodigoBarra { get; set; }

    [BsonElement("imagen")]
    public string? Imagen { get; set; }

    [BsonElement("marca")]
    public string? Marca { get; set; }

    [BsonElement("modelo")]
    public string? Modelo { get; set; }

    [BsonElement("categoria")]
    public string? Categoria { get; set; }

    [BsonElement("color")]
    public string? Color { get; set; }

    [BsonElement("unidadMedida")]
    public string? UnidadMedida { get; set; }

    [BsonElement("esTaller")]
    public bool EsTaller { get; set; }

    [BsonElement("esPieza")]
    public bool EsPieza { get; set; }

    [BsonElement("esProducido")]
    public bool EsProducido { get; set; }

    /// <summary>
    /// Si true, cada unidad física tiene su propio código/serial único (ej: celulares, laptops).
    /// Si false, el producto se maneja por cantidad (ej: cargadores, cables).
    /// </summary>
    [BsonElement("manejaCodigoUnico")]
    public bool ManejaCodigoUnico { get; set; } = false;

    /// <summary>Plantilla de garantía sugerida al asignar a una sucursal.</summary>
    [BsonElement("idPlantillaGarantiaPorDefecto")]
    public string? IdPlantillaGarantiaPorDefecto { get; set; }

    [BsonElement("activo")]
    public bool Activo { get; set; } = true;

    [BsonElement("creadoEnUtc")]
    public DateTime CreadoEnUtc { get; set; } = DateTime.UtcNow;

    [BsonElement("actualizadoEnUtc")]
    public DateTime? ActualizadoEnUtc { get; set; }
}
