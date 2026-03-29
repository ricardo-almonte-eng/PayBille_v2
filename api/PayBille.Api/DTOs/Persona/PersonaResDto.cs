namespace PayBille.Api.DTOs.Persona;

public sealed class PersonaResDto
{
    public string IdPersona { get; set; } = string.Empty;
    public string PrimerNombre { get; set; } = string.Empty;
    public string? Apellido { get; set; }
    public string? Identificacion { get; set; }
    public string? TipoIdentificacion { get; set; }
    public int? IdMarket { get; set; }
    public string? Imagen { get; set; }
    public UsuarioPersonaResDto Usuario { get; set; } = new();
    public DateTime CreadoEnUtc { get; set; }
}

public sealed class UsuarioPersonaResDto
{
    public string NombreUsuario { get; set; } = string.Empty;
    public string? Email { get; set; }
    public int IdRol { get; set; }
    public bool Activo { get; set; }
    public string? Torning { get; set; }
    public bool? Master { get; set; }
}
