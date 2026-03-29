using PayBille.Api.Common;
using PayBille.Api.DTOs.Persona;

namespace PayBille.Api.Interfaces;

public interface IPersonaService
{
    /// <summary>Obtiene todas las personas registradas.</summary>
    Task<Result<List<PersonaResDto>>> ObtenerTodosAsync(CancellationToken ct);

    /// <summary>
    /// Obtiene una persona por su IdPersona.
    /// Returns Fail(PEI01) si no existe.
    /// </summary>
    Task<Result<PersonaResDto>> ObtenerPorIdAsync(string idPersona, CancellationToken ct);

    /// <summary>
    /// Crea una persona nueva (idPersona=null, se genera GUID) o actualiza la existente (idPersona desde ruta).
    /// Returns Fail(PEC02) si la contraseña es requerida al crear y no fue provista.
    /// </summary>
    Task<Result<PersonaResDto>> CrearOActualizarAsync(string? idPersona, PersonaReqDto request, CancellationToken ct);

    /// <summary>
    /// Elimina la persona con el IdPersona indicado.
    /// Returns Fail(PED01) si no existía.
    /// </summary>
    Task<Result<bool>> EliminarAsync(string idPersona, CancellationToken ct);
}
