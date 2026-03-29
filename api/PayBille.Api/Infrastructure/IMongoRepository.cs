using MongoDB.Bson;
using MongoDB.Driver;

namespace PayBille.Api.Infrastructure;

/// <summary>
/// Generic MongoDB repository interface for CRUD operations.
/// </summary>
/// <typeparam name="T">The document type</typeparam>
public interface IMongoRepository<T> where T : class
{
    // ── Create operations ────────────────────────────────────────────────────
    /// <summary>
    /// Inserts a single document.
    /// </summary>
    Task InsertOneAsync(T document, CancellationToken cancellationToken = default);

    /// <summary>
    /// Inserts multiple documents.
    /// </summary>
    Task InsertManyAsync(IEnumerable<T> documents, CancellationToken cancellationToken = default);

    // ── Read operations ──────────────────────────────────────────────────────
    /// <summary>
    /// Finds a document by ID.
    /// </summary>
    Task<T?> FindByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds a document matching the filter.
    /// </summary>
    Task<T?> FindOneAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds all documents matching the filter.
    /// </summary>
    Task<List<T>> FindAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all documents.
    /// </summary>
    Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);

    // ── Update operations ───────────────────────────────────────────────────
    /// <summary>
    /// Updates a single document by ID.
    /// </summary>
    Task<bool> UpdateByIdAsync(string id, UpdateDefinition<T> update, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a single document matching the filter.
    /// </summary>
    Task<bool> UpdateOneAsync(FilterDefinition<T> filter, UpdateDefinition<T> update, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates multiple documents matching the filter.
    /// </summary>
    Task<long> UpdateManyAsync(FilterDefinition<T> filter, UpdateDefinition<T> update, CancellationToken cancellationToken = default);

    // ── Delete operations ───────────────────────────────────────────────────
    /// <summary>
    /// Deletes a document by ID.
    /// </summary>
    Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a document matching the filter.
    /// </summary>
    Task<bool> DeleteOneAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes all documents matching the filter.
    /// </summary>
    Task<long> DeleteManyAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default);

    // ── Count and exists ────────────────────────────────────────────────────
    /// <summary>
    /// Counts documents matching the filter.
    /// </summary>
    Task<long> CountAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a document exists matching the filter.
    /// </summary>
    Task<bool> ExistsAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default);

    // ── Collection access ───────────────────────────────────────────────────
    /// <summary>
    /// Gets direct access to the MongoDB collection for advanced queries.
    /// </summary>
    IMongoCollection<T> Collection { get; }
}
