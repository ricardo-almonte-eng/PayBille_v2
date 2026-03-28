using System.ComponentModel.DataAnnotations;

namespace PayBille.API.DTOs
{
    /// <summary>
    /// Request DTO for [Feature Name]
    /// </summary>
    public class CreateItemRequest
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
    }

    /// <summary>
    /// Response DTO for [Feature Name]
    /// </summary>
    public class ItemResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
