using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Dto.Admin.Products
{
    public class CategoryDto
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
        public string CategoryName { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "Description cannot exceed 250 characters.")]
        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}
