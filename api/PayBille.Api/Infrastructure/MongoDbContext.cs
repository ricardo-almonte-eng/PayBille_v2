using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PayBille.Api.Configuration;

namespace PayBille.Api.Infrastructure;

/// <summary>
/// MongoDB context for managing database connection and collections.
/// </summary>
public sealed class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        var value = settings.Value;
        var client = new MongoClient(value.ConnectionString);
        _database = client.GetDatabase(value.DatabaseName);
    }

    /// <summary>
    /// Gets direct access to the MongoDB database.
    /// </summary>
    public IMongoDatabase Database => _database;

    /// <summary>
    /// Gets a repository for a specific collection.
    /// </summary>
    /// <typeparam name="T">The document type</typeparam>
    /// <param name="collectionName">The collection name in MongoDB</param>
    /// <returns>A repository instance for CRUD operations</returns>
    public IMongoRepository<T> GetRepository<T>(string collectionName) where T : class
    {
        var collection = _database.GetCollection<T>(collectionName);
        return new MongoRepository<T>(collection);
    }
}
