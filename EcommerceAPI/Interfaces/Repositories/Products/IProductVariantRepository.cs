using EcommerceAPI.Models.Products;

namespace EcommerceAPI.Interfaces.Repositories.Products
{
    public interface IProductVariantRepository : IRepository<ProductVariant>
    {
        Task<IEnumerable<ProductVariant>> GetAllVariantsAsync();
        Task<ProductVariant?> GetVariantByIdAsync(Guid id); // 👈 Must be Guid
        Task<bool> DeleteVariantAsync(Guid id);
    }
}
