using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Dto.Admin.Products
{
    public class SubCategoryDto
    {
        public Guid Id { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "SubCategory name cannot exceed 100 characters.")]
        public string SubCategoryName { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "Description cannot exceed 250 characters.")]
        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}
