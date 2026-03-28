namespace PayBille.Api.Services;

public interface IHealthService
{
    Task<object> GetStatusAsync(CancellationToken cancellationToken);
}
