---
name: mongodb-modeling
description: "Design MongoDB document structures for PayBille v2. Use when creating collections, defining schemas, planning indexes, or migrating from relational data."
---

# MongoDB Document Modeling

## When to Use
- Designing new MongoDB collections
- Planning data structure for features
- Optimizing query patterns
- Indexing strategy
- Migrating from MySQL

## Procedure

### 1. Analyze the Domain
- Identify entities and relationships
- Understand query patterns
- Consider read vs write frequency

### 2. Design Document Structure
Use the [model template](./assets/model-template.cs) with proper field names.

### 3. Plan Embedding vs Referencing
- **Embed**: Related data queried together
- **Reference**: Large arrays or 1-to-many relationships

### 4. Create Indexes
Use the [schema template](./assets/collection-schema.json) for index recommendations.

### 5. Validate with Queries
Test common operations before deployment.

## Best Practices
- Use descriptive field names (not single letters)
- Keep document size under 16MB
- Index fields used in filters and sorts
- Denormalize for read performance
- Use ObjectId for relationships

## References
See [MongoDB Best Practices](./references/mongodb-best-practices.md) for advanced strategies.
