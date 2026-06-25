using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Dto.Admin.Products
{
    public class ProductVariantDto
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        [StringLength(250)]
        public string VariantSKU { get; set; } = string.Empty;

        public Guid MaterialId { get; set; }

        [StringLength(20)]
        public string Size { get; set; } = string.Empty;

        public decimal Weight { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
        public decimal DiscountPercentage { get; set; }
        public int StockQuantity { get; set; }
        public List<IFormFile>? Files {get; set; }

        public string ImagesMetadata { get; set; } = string.Empty;

    }
}
