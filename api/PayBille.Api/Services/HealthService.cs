using MongoDB.Bson;
using PayBille.Api.Infrastructure;

namespace PayBille.Api.Services;

public sealed class HealthService : IHealthService
{
    private readonly MongoDbContext _mongoDbContext;

    public HealthService(MongoDbContext mongoDbContext)
    {
        _mongoDbContext = mongoDbContext;
    }

    public async Task<object> GetStatusAsync(CancellationToken cancellationToken)
    {
        var command = new BsonDocument("ping", 1);
        await _mongoDbContext.Database.RunCommandAsync<BsonDocument>(command, cancellationToken: cancellationToken);

        return new
        {
            status = "ok",
            service = "paybille-api",
            database = "mongodb",
            timestampUtc = DateTime.UtcNow
        };
    }
}
