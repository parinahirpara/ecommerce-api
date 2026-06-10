using EcommerceAPI.Dto.Products;
using EcommerceAPI.Models.Products;

namespace EcommerceAPI.Interfaces.Services.Products
{
    public interface ISubCategoryService
    {
        Task<IEnumerable<SubCategory>> GetAllSubCategoriesAsync();
        Task<IEnumerable<SubCategory>> GetSubCategoriesByCategoryIdAsync(int categoryId);
        Task<SubCategory?> GetSubCategoryByIdAsync(int id);
        Task<SubCategory?> CreateSubCategoryAsync(SubCategoryDto dto);
        Task<SubCategory?> UpdateSubCategoryAsync(int id, SubCategoryDto dto);
        Task<bool> DeleteSubCategoryAsync(int id);
    }
}
