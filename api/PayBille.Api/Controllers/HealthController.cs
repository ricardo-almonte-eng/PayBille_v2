using Microsoft.AspNetCore.Mvc;
using PayBille.Api.Services;

namespace PayBille.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class HealthController : ControllerBase
{
    private readonly IHealthService _healthService;

    public HealthController(IHealthService healthService)
    {
        _healthService = healthService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        try
        {
            var status = await _healthService.GetStatusAsync(cancellationToken);
            return Ok(status);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new
            {
                status = "error",
                message = ex.Message,
                timestampUtc = DateTime.UtcNow
            });
        }
    }
}
