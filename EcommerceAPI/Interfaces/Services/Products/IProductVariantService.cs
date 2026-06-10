using EcommerceAPI.Dto.Products;
using EcommerceAPI.Models.Products;

namespace EcommerceAPI.Interfaces.Services.Products
{
    public interface IProductVariantService
    {
        Task<ProductVariant> CreateVariantAsync(ProductVariantDto dto);
        Task<ProductVariant> UpdateVariantAsync(Guid variantId, ProductVariantDto dto);
        Task<IEnumerable<ProductVariant>> GetAllVariantsAsync();
        Task<ProductVariant?> GetVariantByIdAsync(Guid id);
        Task<bool> DeleteVariantAsync(Guid id);
    }
}
