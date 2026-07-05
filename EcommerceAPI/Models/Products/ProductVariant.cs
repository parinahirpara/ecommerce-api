using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceAPI.Models.Products
{
    public class ProductVariant : BaseEntity
    {
        public Guid ProductId { get; set; }
        public string VariantSKU { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;

        public Guid MaterialId { get; set; }
        public decimal Weight { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPercentage { get; set; } 
        public int StockQuantity { get; set; }

        public virtual ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

        [ForeignKey("MaterialId")]
        public virtual Material? Material { get; set; }
    }
}
