using MongoDB.Driver;
using PayBille.Api.Common;
using PayBille.Api.DTOs.Persona;
using PayBille.Api.Errors;
using PayBille.Api.Infrastructure.Repositories;
using PayBille.Api.Interfaces;
using PayBille.Api.Models;

namespace PayBille.Api.Services;

public sealed class PersonaService : IPersonaService
{
    private readonly PersonaRepository _personaRepository;
    private readonly ILogger<PersonaService> _logger;

    public PersonaService(PersonaRepository personaRepository, ILogger<PersonaService> logger)
    {
        _personaRepository = personaRepository;
        _logger            = logger;
    }

    /// <inheritdoc/>
    public async Task<Result<List<PersonaResDto>>> ObtenerTodosAsync(CancellationToken ct)
    {
        var personas = await _personaRepository.GetAllAsync(ct);
        return Result<List<PersonaResDto>>.Ok(personas.ConvertAll(MapToDto));
    }

    /// <inheritdoc/>
    public async Task<Result<PersonaResDto>> ObtenerPorIdAsync(string idPersona, CancellationToken ct)
    {
        var filter  = Builders<Persona>.Filter.Eq(p => p.IdPersona, idPersona);
        var persona = await _personaRepository.FindOneAsync(filter, ct);
        return persona is null
            ? Result<PersonaResDto>.Fail(AppErrors.PersonaNoEncontrada(idPersona))
            : Result<PersonaResDto>.Ok(MapToDto(persona));
    }

    /// <inheritdoc/>
    public async Task<Result<PersonaResDto>> CrearOActualizarAsync(string? idPersona, PersonaReqDto request, CancellationToken ct)
    {
        // Buscar persona existente para preservar Id, CreadoEnUtc y ContrasenaHash si no se cambia.
        Persona? existente = null;
        if (idPersona is not null)
        {
            var filter = Builders<Persona>.Filter.Eq(p => p.IdPersona, idPersona);
            existente  = await _personaRepository.FindOneAsync(filter, ct);
        }

        string contrasenaHash;

        if (!string.IsNullOrWhiteSpace(request.Usuario.Contrasena))
        {
            contrasenaHash = BCrypt.Net.BCrypt.HashPassword(request.Usuario.Contrasena);
        }
        else if (existente is not null)
        {
            contrasenaHash = existente.Usuario.ContrasenaHash;
        }
        else
        {
            return Result<PersonaResDto>.Fail(AppErrors.PersonaContrasenaRequerida());
        }

        var persona = new Persona
        {
            Id                 = existente?.Id ?? MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
            IdPersona          = idPersona ?? Guid.NewGuid().ToString(),
            PrimerNombre       = request.PrimerNombre,
            Apellido           = request.Apellido,
            Identificacion     = request.Identificacion,
            TipoIdentificacion = request.TipoIdentificacion,
            IdMarket           = request.IdMarket,
            Imagen             = request.Imagen,
            CreadoEnUtc        = existente?.CreadoEnUtc ?? DateTime.UtcNow,
            Usuario     = new UsuarioPersona
            {
                NombreUsuario  = request.Usuario.NombreUsuario,
                ContrasenaHash = contrasenaHash,
                Email          = request.Usuario.Email,
                IdRol          = request.Usuario.IdRol,
                Activo         = request.Usuario.Activo,
                Torning        = request.Usuario.Torning,
                Master         = request.Usuario.Master,
                TokensRefresh  = existente?.Usuario.TokensRefresh ?? []
            }
        };

        await _personaRepository.UpsertAsync(persona, ct);

        _logger.LogInformation(
            "Persona {IdPersona} {Accion}.",
            persona.IdPersona,
            existente is null ? "creada" : "actualizada");

        return Result<PersonaResDto>.Ok(MapToDto(persona));
    }

    /// <inheritdoc/>
    public async Task<Result<bool>> EliminarAsync(string idPersona, CancellationToken ct)
    {
        var filter    = Builders<Persona>.Filter.Eq(p => p.IdPersona, idPersona);
        var eliminado = await _personaRepository.DeleteOneAsync(filter, ct);
        return eliminado
            ? Result<bool>.Ok(true)
            : Result<bool>.Fail(AppErrors.PersonaEliminarNoEncontrada(idPersona));
    }

    // ── Mapping ──────────────────────────────────────────────────────────────

    private static PersonaResDto MapToDto(Persona p) => new()
    {
        IdPersona          = p.IdPersona,
        PrimerNombre       = p.PrimerNombre,
        Apellido           = p.Apellido,
        Identificacion     = p.Identificacion,
        TipoIdentificacion = p.TipoIdentificacion,
        IdMarket           = p.IdMarket,
        Imagen             = p.Imagen,
        CreadoEnUtc        = p.CreadoEnUtc,
        Usuario     = new UsuarioPersonaResDto
        {
            NombreUsuario = p.Usuario.NombreUsuario,
            Email         = p.Usuario.Email,
            IdRol         = p.Usuario.IdRol,
            Activo        = p.Usuario.Activo,
            Torning       = p.Usuario.Torning,
            Master        = p.Usuario.Master
        }
    };
}
