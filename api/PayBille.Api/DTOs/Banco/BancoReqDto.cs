namespace PayBille.Api.DTOs.Banco;

/// <summary>Datos de entrada para crear o actualizar un banco en el catálogo.</summary>
public sealed class BancoReqDto
{
    public string  Nombre { get; set; } = string.Empty;
    public string? Codigo { get; set; }
    public bool    Activo { get; set; } = true;
}
