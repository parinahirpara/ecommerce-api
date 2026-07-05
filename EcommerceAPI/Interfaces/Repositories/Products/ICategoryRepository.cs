using EcommerceAPI.Models.Products;

namespace EcommerceAPI.Interfaces.Repositories.Products
{
    public interface ICategoryRepository : IRepository<Category>
    {
        // Add any Category-specific queries here if needed, e.g.:
        Task<IEnumerable<Category>> GetAllWithSubCategoriesAsync();
        Task<(List<Category> Categories, List<Product> Products)> GetMenuConfigurationDataAsync();
    }
}
