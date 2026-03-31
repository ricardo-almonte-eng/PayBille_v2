using System.ComponentModel.DataAnnotations;

namespace PayBille.Api.DTOs.Persona;

public sealed class PersonaReqDto
{
    [Required]
    public string PrimerNombre { get; set; } = string.Empty;

    public string? Apellido { get; set; }

    public string? Identificacion { get; set; }

    public string? TipoIdentificacion { get; set; }

    public int? IdMarket { get; set; }

    [Required]
    public UsuarioPersonaReqDto Usuario { get; set; } = new();
}

public sealed class UsuarioPersonaReqDto
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string NombreUsuario { get; set; } = string.Empty;

    /// <summary>
    /// Contraseña en texto plano. Obligatoria al crear; si se omite al actualizar, conserva la contraseña existente.
    /// </summary>
    public string? Contrasena { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public int IdRol { get; set; }

    public bool Activo { get; set; }

    public string? Torning { get; set; }

    public bool? Master { get; set; }
}
