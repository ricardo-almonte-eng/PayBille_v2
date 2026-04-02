using PayBille.Api.Common;
using PayBille.Api.DTOs.Banco;

namespace PayBille.Api.Interfaces;

public interface IBancoService
{
    /// <summary>Obtiene todos los bancos del catálogo.</summary>
    Task<Result<List<BancoResDto>>> ObtenerTodosAsync(CancellationToken ct);

    /// <summary>
    /// Obtiene un banco por su IdBanco.
    /// Retorna Fail(BAI01) si no existe.
    /// </summary>
    Task<Result<BancoResDto>> ObtenerPorIdAsync(string idBanco, CancellationToken ct);

    /// <summary>
    /// Crea un banco nuevo (IdBanco=null, se genera GUID).
    /// Retorna Fail(BAC02) si ya existe un banco con el mismo nombre.
    /// </summary>
    Task<Result<BancoResDto>> CrearAsync(BancoReqDto request, CancellationToken ct);

    /// <summary>
    /// Actualiza los datos de un banco existente por su IdBanco.
    /// Retorna Fail(BAI01) si no existe.
    /// </summary>
    Task<Result<BancoResDto>> ActualizarAsync(string idBanco, BancoReqDto request, CancellationToken ct);

    /// <summary>
    /// Elimina el banco con el IdBanco indicado.
    /// Retorna Fail(BAD01) si no existía.
    /// </summary>
    Task<Result<bool>> EliminarAsync(string idBanco, CancellationToken ct);
}
