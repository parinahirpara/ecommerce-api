using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceAPI.Models.Products
{
    public class ProductImage : BaseEntity
    {
        [Required]
        [StringLength(500)]
        public string ImageUrl { get; set; } = string.Empty;

        [StringLength(200)]
        public string AltText { get; set; } = string.Empty;

        public int DisplayOrder { get; set; } = 0;

        [Required]
        public Guid ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

        public Guid? ProductVariantId { get; set; }

        [ForeignKey("ProductVariantId")]
        public virtual ProductVariant? ProductVariant { get; set; }

    }
}
