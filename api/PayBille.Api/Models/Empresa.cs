using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PayBille.Api.Models.Enums;

namespace PayBille.Api.Models;

public sealed class Empresa
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("idEmpresa")]
    [BsonRequired]
    public string IdEmpresa { get; set; } = string.Empty;

    [BsonElement("nombre")]
    [BsonRequired]
    public string Nombre { get; set; } = string.Empty;

    [BsonElement("activo")]
    public bool Activo { get; set; }

    [BsonElement("correo")]
    public string? Correo { get; set; }

    [BsonElement("descripcion")]
    public string? Descripcion { get; set; }

    [BsonElement("enlaceCircular")]
    public string? EnlaceCircular { get; set; }

    [BsonElement("direccion")]
    [BsonRequired]
    public string Direccion { get; set; } = string.Empty;

    [BsonElement("direccion2")]
    public string? Direccion2 { get; set; }

    [BsonElement("telefono")]
    public string? Telefono { get; set; }

    [BsonElement("telefono2")]
    public string? Telefono2 { get; set; }

    [BsonElement("idPropietario")]
    [BsonRequired]
    public string IdPropietario { get; set; } = string.Empty;

    [BsonElement("banco")]
    public string? Banco { get; set; }

    [BsonElement("imagen")]
    public string? Imagen { get; set; }

    [BsonElement("rnc")]
    public string? RNC { get; set; }

    [BsonElement("valorImpuesto")]
    public decimal ValorImpuesto { get; set; } = 0;

    [BsonElement("zonaHoraria")]
    public string? ZonaHoraria { get; set; }

    [BsonElement("sucursales")]
    public List<Sucursal> Sucursales { get; set; } = [];

    [BsonElement("creadoEnUtc")]
    public DateTime CreadoEnUtc { get; set; } = DateTime.UtcNow;
}

public sealed class Sucursal
{
    [BsonElement("idSucursal")]
    [BsonRequired]
    public string IdSucursal { get; set; } = string.Empty;

    [BsonElement("nombre")]
    [BsonRequired]
    public string Nombre { get; set; } = string.Empty;

    [BsonElement("direccion")]
    [BsonRequired]
    public string Direccion { get; set; } = string.Empty;

    [BsonElement("direccion2")]
    public string? Direccion2 { get; set; }

    [BsonElement("telefono")]
    public string? Telefono { get; set; }

    [BsonElement("correo")]
    public string? Correo { get; set; }

    [BsonElement("estatus")]
    public EstatusSucursal Estatus { get; set; } = EstatusSucursal.Abierta;

    [BsonElement("creadoEnUtc")]
    public DateTime CreadoEnUtc { get; set; } = DateTime.UtcNow;
}
