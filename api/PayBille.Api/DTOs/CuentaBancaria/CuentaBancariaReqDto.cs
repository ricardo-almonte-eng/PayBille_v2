using PayBille.Api.Models.Enums;

namespace PayBille.Api.DTOs.CuentaBancaria;

/// <summary>Datos de entrada para crear o actualizar una cuenta bancaria.</summary>
public sealed class CuentaBancariaReqDto
{
    public string     IdEmpresa    { get; set; } = string.Empty;
    public string     IdBanco      { get; set; } = string.Empty;
    public string     Nombre       { get; set; } = string.Empty;
    public string     NumeroCuenta { get; set; } = string.Empty;
    public TipoCuenta TipoCuenta   { get; set; } = TipoCuenta.Corriente;
    public string     Moneda       { get; set; } = "DOP";
    public bool       Activo       { get; set; } = true;
    public string?    Descripcion  { get; set; }
}
