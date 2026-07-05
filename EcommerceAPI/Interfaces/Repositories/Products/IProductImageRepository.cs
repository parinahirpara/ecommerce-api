using EcommerceAPI.Models.Products;

namespace EcommerceAPI.Interfaces.Repositories.Products
{
    public interface IProductImageRepository : IRepository<ProductImage>
    {
        Task<IEnumerable<ProductImage>> GetImagesByVariantIdAsync(Guid variantId);
    }
}
