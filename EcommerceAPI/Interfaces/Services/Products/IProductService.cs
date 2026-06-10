using EcommerceAPI.Dto.Products;
using EcommerceAPI.Models.Products;

namespace EcommerceAPI.Interfaces.Services.Products
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(Guid id);
        Task AddAsync(ProductDto dto);
        Task UpdateAsync(Guid id, ProductDto dto);
        Task DeleteAsync(Guid id);

    }
}
