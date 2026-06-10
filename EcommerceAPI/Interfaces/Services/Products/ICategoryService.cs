using EcommerceAPI.Dto.Products;
using EcommerceAPI.Models.Products;

namespace EcommerceAPI.Interfaces.Services.Products
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<Category> CreateCategoryAsync(CategoryDto dto);
        Task<Category?> UpdateCategoryAsync(int id, CategoryDto dto);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
