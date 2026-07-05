using EcommerceAPI.Dto.Common;
using EcommerceAPI.Models.Products;

namespace EcommerceAPI.Interfaces.Repositories.Products
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllWithDetailsAsync();
        Task<Product?> GetWithDetailsAsync(Guid id);
        Task<Product> GetProductDetailsByIdAsync(Guid id);
        Task<PagedResult<Product>> GetPagedProductWithDetailsAsync(int skip, int take,Guid? categoryId, Guid? subCategoryId, Guid? materialId);
    }
}
