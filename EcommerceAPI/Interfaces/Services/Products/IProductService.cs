using Admin = EcommerceAPI.Dto.Admin.Products;
using Customer = EcommerceAPI.Dto.Customer.Products;
using EcommerceAPI.Dto.Common;

namespace EcommerceAPI.Interfaces.Services.Products
{
    public interface IProductService
    {
        Task<IEnumerable<Admin.ProductDto>> GetAllAsync();
        Task<Admin.ProductDto?> GetByIdAsync(Guid id);
        Task<Admin.ProductDto> AddAsync(Admin.ProductDto dto);
        Task UpdateAsync(Guid id, Admin.ProductDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<PagedResult<Customer.ProductResponseDto>> GetProductsAsync(int skip, int take, Guid? categoryId, Guid? subCategoryId, Guid? materialId);

    }
}
