namespace PayBille.Api.DTOs.Catalogo;

/// <summary>
/// Elemento de zona horaria para llenar un select en el cliente.
/// </summary>
public sealed class ZonaHorariaItemDto
{
    /// <summary>Valor entero del enum <see cref="Models.Enums.ZonaHoraria"/>.</summary>
    public int Valor { get; init; }

    /// <summary>Etiqueta legible para mostrar al usuario, incluye el desplazamiento UTC.</summary>
    public string Etiqueta { get; init; } = string.Empty;

    /// <summary>Identificador IANA de la zona horaria (p. ej. "America/Santo_Domingo").</summary>
    public string IanaId { get; init; } = string.Empty;

    /// <summary>Desplazamiento UTC en formato ±HH:mm (p. ej. "-04:00").</summary>
    public string DesplazamientoUtc { get; init; } = string.Empty;
}
