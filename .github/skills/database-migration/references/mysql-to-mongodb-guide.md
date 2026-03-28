# MySQL to MongoDB Migration Guide

## Mapping Strategy

### Tables → Collections
- One MySQL table = One MongoDB collection
- Use plural names: `products`, `customers`, `orders`

### Columns → Fields
- Use camelCase for field names
- Keep same data types where possible
- Convert DATETIME to ISODate

### Foreign Keys → References or Embedding
- Small related data: Embed
- Large arrays: Use ObjectId references

### Example: Products Table

**MySQL:**
```sql
CREATE TABLE products (
  id INT PRIMARY KEY AUTO_INCREMENT,
  name VARCHAR(100),
  sku VARCHAR(50) UNIQUE,
  price DECIMAL(10,2),
  category_id INT,
  created_at DATETIME
);
```

**MongoDB:**
```javascript
{
  _id: ObjectId,
  name: String,
  sku: String,    // Create unique index
  price: Decimal128,
  categoryId: ObjectId,  // Reference to categories collection
  createdAt: ISODate
}
```

## Data Type Mapping

| MySQL | MongoDB |
|-------|---------|
| INT | Int32 |
| BIGINT | Int64 |
| DECIMAL | Decimal128 |
| VARCHAR | String |
| TEXT | String |
| BOOLEAN | Boolean |
| DATETIME | ISODate |
| DATE | ISODate |
| NULL | Null / omit field |

## Validation Checklist

- [ ] All tables migrated
- [ ] Row counts match
- [ ] Sample records validated
- [ ] Unique indexes created
- [ ] Compound indexes for queries
- [ ] Text indexes for search
- [ ] Schema validation enabled
- [ ] Referential integrity tested

## Common Pitfalls

### Losing Data in Type Conversion
- Always validate data before insertion
- Test with sample rows first
- Keep backup of source data

### Performance Issues
- Use batch operations (insertMany)
- Create indexes AFTER migration
- Test on small dataset first

### Relationship Handling
- Decide embedding vs referencing early
- Update all references when changing IDs
- Document relationship structure
