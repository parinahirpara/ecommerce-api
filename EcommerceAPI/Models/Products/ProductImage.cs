using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceAPI.Models.Products
{
    public class ProductImage
    {

        [Key]
        public int ImageId { get; set; }

        // The relative path or cloud storage URL (e.g., "/uploads/products/engagement-ring-1.jpg")
        [Required]
        [StringLength(500)]
        public string ImageUrl { get; set; } = string.Empty;

        // Image alt text for SEO optimization and screen-reader accessibility
        [StringLength(200)]
        public string AltText { get; set; } = string.Empty;

        // Sequence number to sort the thumbnail sequence in your storefront gallery carousel
        public int DisplayOrder { get; set; } = 0;

        // Foreign Key pointing to the main Product
        [Required]
        public Guid ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

        // Optional Foreign Key: Assign here if the image is exclusive to a specific color/metal variant
        public Guid? ProductVariantId { get; set; }

        [ForeignKey("ProductVariantId")]
        public virtual ProductVariant? ProductVariant { get; set; }

    }
}
