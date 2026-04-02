using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PayBille.Api.Models.Enums;

namespace PayBille.Api.Models;

/// <summary>
/// Cuenta bancaria de una empresa. Una empresa puede tener múltiples cuentas.
/// Referencia al catálogo de bancos y desnormaliza el nombre del banco para eficiencia.
/// </summary>
public sealed class CuentaBancaria
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("idCuentaBancaria")]
    [BsonRequired]
    public string IdCuentaBancaria { get; set; } = string.Empty;

    [BsonElement("idEmpresa")]
    [BsonRequired]
    public string IdEmpresa { get; set; } = string.Empty;

    [BsonElement("idBanco")]
    [BsonRequired]
    public string IdBanco { get; set; } = string.Empty;

    /// <summary>Nombre del banco desnormalizado para mostrar sin hacer join.</summary>
    [BsonElement("nombreBanco")]
    public string NombreBanco { get; set; } = string.Empty;

    [BsonElement("nombre")]
    [BsonRequired]
    public string Nombre { get; set; } = string.Empty;

    [BsonElement("numeroCuenta")]
    [BsonRequired]
    public string NumeroCuenta { get; set; } = string.Empty;

    [BsonElement("tipoCuenta")]
    public TipoCuenta TipoCuenta { get; set; } = TipoCuenta.Corriente;

    [BsonElement("moneda")]
    public string Moneda { get; set; } = "DOP";

    [BsonElement("activo")]
    public bool Activo { get; set; } = true;

    [BsonElement("descripcion")]
    public string? Descripcion { get; set; }

    [BsonElement("creadoEnUtc")]
    public DateTime CreadoEnUtc { get; set; } = DateTime.UtcNow;
}
