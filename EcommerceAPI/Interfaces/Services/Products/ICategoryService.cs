using EcommerceAPI.Dto.Admin.Products;
using EcommerceAPI.Models.Products;

namespace EcommerceAPI.Interfaces.Services.Products
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(Guid id);
        Task<CategoryDto> CreateCategoryAsync(CategoryDto dto);
        Task<CategoryDto?> UpdateCategoryAsync(Guid id, CategoryDto dto);
        Task<bool> DeleteCategoryAsync(Guid id);
    }
}
