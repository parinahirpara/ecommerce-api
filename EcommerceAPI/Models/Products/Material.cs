using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Models.Products
{
    public class Material : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string MaterialName { get; set; } = string.Empty; 
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
