using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceAPI.Models.Products
{
    public class Product : BaseEntity
    {
        public string ProductSKU { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public string ProductShortDescription { get; set; } = string.Empty;
        public string ProductBrand { get; set; } = string.Empty;
        public Guid CategoryId { get; set; } 
        public Guid SubCategoryId { get; set; }  
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category? Category { get; set; }

        [ForeignKey(nameof(SubCategoryId))]
        public virtual SubCategory? SubCategory { get; set; }

        public virtual ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
        public virtual ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    }
}
