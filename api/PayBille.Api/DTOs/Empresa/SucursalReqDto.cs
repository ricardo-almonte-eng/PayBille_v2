namespace PayBille.Api.DTOs.Empresa;

public sealed class SucursalReqDto
{
    public string Nombre { get; set; } = string.Empty;

    public string Direccion { get; set; } = string.Empty;

    public string? Direccion2 { get; set; }

    public string? Telefono { get; set; }

    public string? Correo { get; set; }
}
