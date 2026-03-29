using MongoDB.Driver;
using PayBille.Api.Common;
using PayBille.Api.DTOs.Market;
using PayBille.Api.Errors;
using PayBille.Api.Infrastructure.Repositories;
using PayBille.Api.Interfaces;
using PayBille.Api.Models;

namespace PayBille.Api.Services;

public sealed class MarketService : IMarketService
{
    private readonly MarketRepository _marketRepository;
    private readonly ILogger<MarketService> _logger;

    public MarketService(MarketRepository marketRepository, ILogger<MarketService> logger)
    {
        _marketRepository = marketRepository;
        _logger           = logger;
    }

    /// <inheritdoc/>
    public async Task<Result<List<MarketResDto>>> ObtenerTodosAsync(CancellationToken ct)
    {
        var markets = await _marketRepository.GetAllAsync(ct);
        return Result<List<MarketResDto>>.Ok(markets.ConvertAll(MapToDto));
    }

    /// <inheritdoc/>
    public async Task<Result<MarketResDto>> ObtenerPorIdAsync(string idMarket, CancellationToken ct)
    {
        var filter = Builders<Market>.Filter.Eq(m => m.IdMarket, idMarket);
        var market = await _marketRepository.FindOneAsync(filter, ct);
        return market is null
            ? Result<MarketResDto>.Fail(AppErrors.MarketNoEncontrado(idMarket))
            : Result<MarketResDto>.Ok(MapToDto(market));
    }

    /// <inheritdoc/>
    public async Task<Result<MarketResDto>> CrearOActualizarAsync(string? idMarket, MarketReqDto request, CancellationToken ct)
    {
        Market? existente = null;
        if (idMarket is not null)
        {
            var filter = Builders<Market>.Filter.Eq(m => m.IdMarket, idMarket);
            existente  = await _marketRepository.FindOneAsync(filter, ct);
        }

        var market = new Market
        {
            Id           = existente?.Id ?? MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
            IdMarket     = idMarket ?? Guid.NewGuid().ToString(),
            Name         = request.Name,
            Active       = request.Active,
            Mail         = request.Mail,
            Description  = request.Description,
            CircularLink = request.CircularLink,
            Address      = request.Address,
            Address2     = request.Address2,
            Phone        = request.Phone,
            Phone2       = request.Phone2,
            IdOwner      = request.IdOwner,
            Bank         = request.Bank,
            Image        = request.Image,
            RNC          = request.RNC,
            TaxValue     = request.TaxValue,
            CreadoEnUtc  = existente?.CreadoEnUtc ?? DateTime.UtcNow,
        };

        await _marketRepository.UpsertAsync(market, ct);

        _logger.LogInformation(
            "Market {IdMarket} {Accion}.",
            market.IdMarket,
            existente is null ? "creado" : "actualizado");

        return Result<MarketResDto>.Ok(MapToDto(market));
    }

    /// <inheritdoc/>
    public async Task<Result<bool>> EliminarAsync(string idMarket, CancellationToken ct)
    {
        var filter    = Builders<Market>.Filter.Eq(m => m.IdMarket, idMarket);
        var eliminado = await _marketRepository.DeleteOneAsync(filter, ct);
        return eliminado
            ? Result<bool>.Ok(true)
            : Result<bool>.Fail(AppErrors.MarketEliminarNoEncontrado(idMarket));
    }

    // ── Mapping ──────────────────────────────────────────────────────────────

    private static MarketResDto MapToDto(Market m) => new()
    {
        IdMarket     = m.IdMarket,
        Name         = m.Name,
        Active       = m.Active,
        Mail         = m.Mail,
        Description  = m.Description,
        CircularLink = m.CircularLink,
        Address      = m.Address,
        Address2     = m.Address2,
        Phone        = m.Phone,
        Phone2       = m.Phone2,
        IdOwner      = m.IdOwner,
        Bank         = m.Bank,
        Image        = m.Image,
        RNC          = m.RNC,
        TaxValue     = m.TaxValue,
        CreadoEnUtc  = m.CreadoEnUtc,
    };
}
