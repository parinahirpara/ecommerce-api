using EcommerceAPI.Data;
using EcommerceAPI.Interfaces.Repositories.Products;
using EcommerceAPI.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Repositories.Products
{
    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        // In ProductImageRepository.cs
        public async Task<IEnumerable<ProductImage>> GetImagesByVariantIdAsync(Guid variantId)
        {
            return await _dbContextSet
                .Where(x => x.ProductVariantId == variantId)
                .ToListAsync<ProductImage>();
        }
    }
}
