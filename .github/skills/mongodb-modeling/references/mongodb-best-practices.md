# MongoDB Best Practices for PayBille v2

## Document Size
Keep documents under 16MB. Use references for large arrays.

## Indexing Strategy
- Index fields used in queries
- Compound indexes for common filter combinations
- Avoid over-indexing (impacts write performance)

## Data Types
- Use ObjectId for relationships
- Use ISODate for timestamps
- Keep bool and enum fields small

## Query Optimization
- Use projection to limit returned fields
- Sort on indexed fields
- Aggregation pipeline for complex queries

## Data Integrity
- Use schema validation
- Implement application-level validation
- Consider multi-document transactions for complex updates

## Denormalization Patterns

### Embed Related Data
Use when:
- Data is accessed together
- Relationship is 1-to-few
- Data doesn't change frequently

Example:
```json
{
  "_id": ObjectId,
  "name": "Product",
  "supplier": {
    "id": ObjectId,
    "name": "Supplier Name",
    "leadDays": 5
  }
}
```

### Reference with ObjectId
Use when:
- Data is large or frequently updated
- Many-to-many relationship
- Relationship is 1-to-many

Example:
```json
{
  "_id": ObjectId,
  "name": "Order",
  "customerId": ObjectId,
  "items": [ObjectId, ObjectId, ...]
}
```

## TTL Indexes
Create expiring documents for temporary data:
```json
{
  "createdAt": ISODate,
  "expiresAt": ISODate("2026-03-27T23:59:59Z")
}
```

With index: `{ "expiresAt": 1 } with TTL of 0 seconds`
