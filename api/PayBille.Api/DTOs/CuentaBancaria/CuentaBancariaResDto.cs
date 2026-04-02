using PayBille.Api.Models.Enums;

namespace PayBille.Api.DTOs.CuentaBancaria;

/// <summary>Datos de respuesta para una cuenta bancaria.</summary>
public sealed class CuentaBancariaResDto
{
    public string    IdCuentaBancaria      { get; set; } = string.Empty;
    public string    IdEmpresa             { get; set; } = string.Empty;
    public string    IdBanco               { get; set; } = string.Empty;
    public string    NombreBanco           { get; set; } = string.Empty;
    public string    Nombre                { get; set; } = string.Empty;
    public string    NumeroCuenta          { get; set; } = string.Empty;
    public TipoCuenta TipoCuenta           { get; set; }
    public string    TipoCuentaDescripcion => TipoCuenta.ToString();
    public string    Moneda                { get; set; } = string.Empty;
    public bool      Activo                { get; set; }
    public string?   Descripcion           { get; set; }
    public DateTime  CreadoEnUtc           { get; set; }
}
