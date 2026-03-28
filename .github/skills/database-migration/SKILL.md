---
name: database-migration
description: "Migrate data from MySQL to MongoDB for PayBille v2. Use when transforming schema, validating data integrity, or planning migration strategy."
---

# MySQL to MongoDB Migration

## When to Use
- Planning data migration from MySQL
- Transforming table data to documents
- Validating migrated data
- Writing migration scripts
- Troubleshooting data inconsistencies

## Procedure

### 1. Analyze Current MySQL Schema
- Document tables and relationships
- Understand foreign keys and constraints
- Identify denormalization opportunities

### 2. Design MongoDB Documents
- Plan document structure
- Decide embedding vs referencing
- Define indexes

### 3. Write Migration Script
Use [helper script](./assets/migration-helper.ts) to load and transform data.

### 4. Validate Data
Use [validation script](./assets/validation-script.ts) to verify integrity.

### 5. For Each Table
1. Transform structure
2. Load into MongoDB
3. Validate count and sample records
4. Index MongoDB collection

## Best Practices
- Backup MySQL before migration
- Migrate in batches for large tables
- Validate row counts and checksums
- Test with sample data first
- Enable MongoDB validation rules

## References
See [MySQL to MongoDB Guide](./references/mysql-to-mongodb-guide.md) for mapping patterns.
