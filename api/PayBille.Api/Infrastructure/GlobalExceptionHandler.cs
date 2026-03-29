using Microsoft.AspNetCore.Diagnostics;
using PayBille.Api.DTOs;
using PayBille.Api.Errors;

namespace PayBille.Api.Infrastructure;

/// <summary>
/// Captura globalmente cualquier excepción no controlada.
/// Retorna HTTP 500 con el código APC01 — el único caso en que la API devuelve 500.
/// </summary>
public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Excepción no controlada capturada por GlobalExceptionHandler.");

        httpContext.Response.StatusCode  = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsJsonAsync(
            ApiRespDto<object>.Error(AppErrors.ErrorCriticoInterno()),
            cancellationToken);

        return true;
    }
}
