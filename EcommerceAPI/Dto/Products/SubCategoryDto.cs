using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Dto.Products
{
    public class SubCategoryDto
    {
        [Required]
        public int CategoryId { get; set; } // The target Parent Category ID

        [Required]
        [StringLength(100, ErrorMessage = "SubCategory name cannot exceed 100 characters.")]
        public string SubCategoryName { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "Description cannot exceed 250 characters.")]
        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}
