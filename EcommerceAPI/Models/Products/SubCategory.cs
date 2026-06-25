using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceAPI.Models.Products
{
    public class SubCategory : BaseEntity
    {
        public Guid CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string SubCategoryName { get; set; } = string.Empty; // e.g., "Diamond Rings" or "Silver Bracelets"

        [StringLength(250)]
        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

    }
}
