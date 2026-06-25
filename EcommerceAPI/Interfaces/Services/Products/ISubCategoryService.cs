using EcommerceAPI.Dto.Admin.Products;
using EcommerceAPI.Models.Products;

namespace EcommerceAPI.Interfaces.Services.Products
{
    public interface ISubCategoryService
    {
        Task<IEnumerable<SubCategoryDto>> GetAllSubCategoriesAsync();
        Task<IEnumerable<SubCategoryDto>> GetSubCategoriesByCategoryIdAsync(Guid categoryId);
        Task<SubCategoryDto?> GetSubCategoryByIdAsync(Guid id);
        Task<SubCategoryDto?> CreateSubCategoryAsync(SubCategoryDto dto);
        Task<SubCategoryDto?> UpdateSubCategoryAsync(Guid id, SubCategoryDto dto);
        Task<bool> DeleteSubCategoryAsync(Guid id);
    }
}
