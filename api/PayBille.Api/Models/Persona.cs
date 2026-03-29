using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PayBille.Api.Models;

public sealed class Persona
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("idPersona")]
    [BsonRequired]
    public string IdPersona { get; set; } = string.Empty;

    [BsonElement("primerNombre")]
    [BsonRequired]
    public string PrimerNombre { get; set; } = string.Empty;

    [BsonElement("apellido")]
    public string? Apellido { get; set; }

    [BsonElement("identificacion")]
    public string? Identificacion { get; set; }

    [BsonElement("tipoIdentificacion")]
    public string? TipoIdentificacion { get; set; }

    [BsonElement("idMarket")]
    public int? IdMarket { get; set; }

    [BsonElement("imagen")]
    public string? Imagen { get; set; }

    [BsonElement("usuario")]
    public UsuarioPersona Usuario { get; set; } = new();

    [BsonElement("creadoEnUtc")]
    public DateTime CreadoEnUtc { get; set; } = DateTime.UtcNow;
}

public sealed class UsuarioPersona
{
    [BsonElement("nombreUsuario")]
    public string NombreUsuario { get; set; } = string.Empty;

    [BsonElement("contrasenaHash")]
    public string ContrasenaHash { get; set; } = string.Empty;

    [BsonElement("email")]
    public string? Email { get; set; }

    [BsonElement("idRol")]
    public int IdRol { get; set; }

    [BsonElement("activo")]
    public bool Activo { get; set; }

    [BsonElement("torning")]
    public string? Torning { get; set; }

    [BsonElement("master")]
    public bool? Master { get; set; }

    [BsonElement("tokensRefresh")]
    public List<RefreshToken> TokensRefresh { get; set; } = [];
}