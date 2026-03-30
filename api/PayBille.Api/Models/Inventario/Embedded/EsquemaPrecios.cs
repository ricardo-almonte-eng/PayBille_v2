using MongoDB.Bson.Serialization.Attributes;

namespace PayBille.Api.Models.Inventario.Embedded;

/// <summary>Precios configurados por la sucursal para este producto.</summary>
public sealed class EsquemaPrecios
{
    [BsonElement("precio1")] public decimal Precio1 { get; set; } = 0;
    [BsonElement("precio2")] public decimal? Precio2 { get; set; }
    [BsonElement("precio3")] public decimal? Precio3 { get; set; }
    [BsonElement("precio4")] public decimal? Precio4 { get; set; }

    [BsonElement("impuesto1")] public decimal? Impuesto1 { get; set; }
    [BsonElement("impuesto2")] public decimal? Impuesto2 { get; set; }
    [BsonElement("impuesto3")] public decimal? Impuesto3 { get; set; }
    [BsonElement("impuesto4")] public decimal? Impuesto4 { get; set; }

    [BsonElement("utilidad1")] public decimal? Utilidad1 { get; set; }
    [BsonElement("utilidad2")] public decimal? Utilidad2 { get; set; }
    [BsonElement("utilidad3")] public decimal? Utilidad3 { get; set; }
    [BsonElement("utilidad4")] public decimal? Utilidad4 { get; set; }

    [BsonElement("rangoMin")]      public decimal RangoMin { get; set; } = 1;
    [BsonElement("rangoMax")]      public decimal RangoMax { get; set; } = 1;
    [BsonElement("rangoImpuesto")] public decimal RangoImpuesto { get; set; } = 0;

    [BsonElement("precioEspecial")]  public decimal? PrecioEspecial { get; set; }
    [BsonElement("inicioEspecial")]  public DateTime? InicioEspecial { get; set; }
    [BsonElement("finEspecial")]     public DateTime? FinEspecial { get; set; }

    [BsonElement("precioManager")] public decimal? PrecioManager { get; set; }

    /// <summary>Precio de venta al detalle (por debajo de la unidad mínima).</summary>
    [BsonElement("alPorMenorActivo")]          public bool AlPorMenorActivo { get; set; } = false;
    [BsonElement("alPorMenorCantidadMinima")]   public decimal? AlPorMenorCantidadMinima { get; set; }
    [BsonElement("alPorMenorCantidadMaxima")]   public decimal? AlPorMenorCantidadMaxima { get; set; }
}
