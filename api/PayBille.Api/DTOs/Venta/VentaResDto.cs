using PayBille.Api.Models.Enums;

namespace PayBille.Api.DTOs.Venta;

/// <summary>
/// Respuesta con los datos de una venta registrada.
/// </summary>
public sealed class VentaResDto
{
    public string       Id              { get; set; } = string.Empty;
    public string       IdEmpresa       { get; set; } = string.Empty;
    public string       IdSucursal      { get; set; } = string.Empty;
    public string?      IdCliente       { get; set; }
    public string?      NombreCliente   { get; set; }
    public decimal      DescuentoCliente { get; set; }
    public string?      Ncf             { get; set; }
    public string?      Rnc             { get; set; }
    public DateTime     Fecha           { get; set; }
    public DateTime     CreadoEnUtc     { get; set; }
    public EstatusVenta Estatus         { get; set; }
    public decimal      Efectivo        { get; set; }
    public decimal      Cambio          { get; set; }
    public decimal      Deposito        { get; set; }
    public decimal      Credito         { get; set; }
    public decimal      Canje           { get; set; }
    public decimal      Financiamiento  { get; set; }
    public decimal      SubTotal        { get; set; }
    public decimal      Impuesto        { get; set; }
    public decimal      Total           { get; set; }
    public string?      Nota            { get; set; }
    public string?      IdPersona       { get; set; }
    public string?      NombreUsuario   { get; set; }
    public string?      IdTurno         { get; set; }
    public List<ItemVentaResDto> Items  { get; set; } = [];
}
