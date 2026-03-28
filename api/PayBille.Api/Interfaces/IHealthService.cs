namespace PayBille.Api.Interfaces;

public interface IHealthService
{
    Task<object> GetStatusAsync(CancellationToken cancellationToken);
}
