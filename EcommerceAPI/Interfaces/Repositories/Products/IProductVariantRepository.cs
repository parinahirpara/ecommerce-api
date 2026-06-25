using EcommerceAPI.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Interfaces.Repositories.Products
{
    public interface IProductVariantRepository : IRepository<ProductVariant>
    {
        Task<IEnumerable<ProductVariant>> GetAllVariantsAsync();
        Task<ProductVariant?> GetVariantByIdAsync(Guid id); // 👈 Must be Guid
        Task<bool> DeleteVariantAsync(Guid id);
      
    }
}
