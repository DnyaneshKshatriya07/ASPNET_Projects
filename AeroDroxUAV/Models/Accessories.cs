using System.ComponentModel.DataAnnotations.Schema;

namespace AeroDroxUAV.Models
{
    public class Accessories
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public double Price { get; set; }
        public double? DiscountPrice { get; set; } // New field
        public string? Description { get; set; }
        public string? Condition { get; set; }
        public int StockQuantity { get; set; } = 0;
        public bool ShowOnHomepage { get; set; } = false; // NEW FIELD: Add to Home Page
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Image properties
        public string? ImageUrl { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}