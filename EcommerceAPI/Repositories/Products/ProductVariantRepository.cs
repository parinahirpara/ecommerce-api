using EcommerceAPI.Data;
using EcommerceAPI.Interfaces.Repositories.Products;
using EcommerceAPI.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Repositories.Products
{
   public class ProductVariantRepository : Repository<ProductVariant>, IProductVariantRepository
    {
        public ProductVariantRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<ProductVariant>> GetAllVariantsAsync()
        {
            return await _dbContext.ProductVariants
                .Include(pv => pv.Product)
                .Include(pv => pv.Material)
                .Include(pv => pv.Images)
                .OrderByDescending(pv => pv.Id)
                .ToListAsync();
        }

        public async Task<ProductVariant> GetVariantByIdAsync(Guid id)
        {
            return await _dbContext.ProductVariants
                .Include(pv => pv.Images)
                .FirstOrDefaultAsync(pv => pv.Id == id);
        }

        public async Task<bool> DeleteVariantAsync(Guid id) 
        {
            var variant = await GetVariantByIdAsync(id);

            if (variant == null)
            {
                return false;
            }

            _dbContext.ProductVariants.Remove(variant);

            var rowsAffected = await _dbContext.SaveChangesAsync();
            return rowsAffected > 0;
        }

    }
}
