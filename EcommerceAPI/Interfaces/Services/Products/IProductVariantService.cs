using EcommerceAPI.Dto.Admin.Products;
using EcommerceAPI.Models.Products;

namespace EcommerceAPI.Interfaces.Services.Products
{
    public interface IProductVariantService
    {
        Task<IEnumerable<ProductVariantResponseDto>> GetAllVariantsAsync();
        Task<ProductVariantResponseDto?> GetVariantByIdAsync(Guid id);
        Task<ProductVariantResponseDto> CreateVariantAsync(ProductVariantDto dto);
        Task<ProductVariantResponseDto> UpdateVariantAsync(Guid variantId, ProductVariantDto dto);
        Task<bool> DeleteVariantAsync(Guid id);
    }
}
