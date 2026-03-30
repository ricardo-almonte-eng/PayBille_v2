using PayBille.Api.Models.Enums;

namespace PayBille.Api.DTOs.Empresa;

public sealed class EmpresaResDto
{
    public string IdEmpresa { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public bool Activo { get; set; }
    public string? Correo { get; set; }
    public string? Descripcion { get; set; }
    public string? EnlaceCircular { get; set; }
    public string Direccion { get; set; } = string.Empty;
    public string? Direccion2 { get; set; }
    public string? Telefono { get; set; }
    public string? Telefono2 { get; set; }
    public string IdPropietario { get; set; } = string.Empty;
    public string? Banco { get; set; }
    public string? Imagen { get; set; }
    public string? RNC { get; set; }
    public decimal ValorImpuesto { get; set; }
    public List<SucursalResDto> Sucursales { get; set; } = [];
    public DateTime CreadoEnUtc { get; set; }
}

public sealed class SucursalResDto
{
    public string IdSucursal { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string? Direccion2 { get; set; }
    public string? Telefono { get; set; }
    public string? Correo { get; set; }
    public EstatusSucursal Estatus { get; set; }
    public string EstatusDescripcion => Estatus.ToString();
    public DateTime CreadoEnUtc { get; set; }
}
