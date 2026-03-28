using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PayBille.Api.Configuration;

namespace PayBille.Api.Infrastructure;

public sealed class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var value = settings.Value;
        var client = new MongoClient(value.ConnectionString);
        _database = client.GetDatabase(value.DatabaseName);
    }

    public IMongoDatabase Database => _database;
}
