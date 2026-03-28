using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PayBille.Core.Models
{
    [BsonIgnoreExtraElements]
    public class Product
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("sku")]
        public string SKU { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("cost")]
        public decimal Cost { get; set; }

        [BsonElement("stock")]
        public int StockQuantity { get; set; }

        [BsonElement("category")]
        public string CategoryId { get; set; }

        [BsonElement("supplier")]
        public SupplierInfo SupplierInfo { get; set; }

        [BsonElement("status")]
        public string Status { get; set; } // active, inactive

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class SupplierInfo
    {
        [BsonElement("supplierId")]
        public string SupplierId { get; set; }

        [BsonElement("supplierName")]
        public string SupplierName { get; set; }

        [BsonElement("leadDays")]
        public int LeadDays { get; set; }
    }
}
