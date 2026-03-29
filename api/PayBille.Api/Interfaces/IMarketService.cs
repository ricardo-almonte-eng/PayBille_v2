using PayBille.Api.Common;
using PayBille.Api.DTOs.Market;

namespace PayBille.Api.Interfaces;

public interface IMarketService
{
    /// <summary>Obtiene todos los markets registrados.</summary>
    Task<Result<List<MarketResDto>>> ObtenerTodosAsync(CancellationToken ct);

    /// <summary>
    /// Obtiene un market por su IdMarket.
    /// Returns Fail(MAI01) si no existe.
    /// </summary>
    Task<Result<MarketResDto>> ObtenerPorIdAsync(string idMarket, CancellationToken ct);

    /// <summary>
    /// Crea un market nuevo (idMarket=null, se genera GUID) o actualiza el existente (idMarket desde ruta).
    /// </summary>
    Task<Result<MarketResDto>> CrearOActualizarAsync(string? idMarket, MarketReqDto request, CancellationToken ct);

    /// <summary>
    /// Elimina el market con el IdMarket indicado.
    /// Returns Fail(MAD01) si no existía.
    /// </summary>
    Task<Result<bool>> EliminarAsync(string idMarket, CancellationToken ct);
}
