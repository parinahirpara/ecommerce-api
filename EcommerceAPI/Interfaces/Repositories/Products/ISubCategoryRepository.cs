using EcommerceAPI.Models.Products;

namespace EcommerceAPI.Interfaces.Repositories.Products
{
    public interface ISubCategoryRepository : IRepository<SubCategory>
    {
        Task<IEnumerable<SubCategory>> GetSubCategoriesByCategoryIdAsync(Guid categoryId);
        Task<IEnumerable<SubCategory>> GetAllWithCategoryAsync();
    }
}
