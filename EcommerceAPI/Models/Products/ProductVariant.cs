using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceAPI.Models.Products
{
    public class ProductVariant
    {
        [Key]
        public Guid VariantId { get; set; }

        // Foreign Key referencing the Master Table
        public Guid ProductId { get; set; }

        public string VariantSKU { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public decimal Weight { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPercentage { get; set; } // e.g., 10.00 for 10%
        public int StockQuantity { get; set; }

        public virtual ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
    }
}
