namespace PayBille.Api.DTOs.Banco;

/// <summary>Datos de respuesta para un banco del catálogo.</summary>
public sealed class BancoResDto
{
    public string   IdBanco     { get; set; } = string.Empty;
    public string   Nombre      { get; set; } = string.Empty;
    public string?  Codigo      { get; set; }
    public bool     Activo      { get; set; }
    public DateTime CreadoEnUtc { get; set; }
}
