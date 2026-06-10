using EcommerceAPI.Data;
using EcommerceAPI.Interfaces.Repositories.Products;
using EcommerceAPI.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Repositories.Products
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Product>> GetAllWithDetailsAsync()
        {
            return await _dbContextSet
                .Include(p => p.Images)
                .Include(p => p.Variants)
                    .ThenInclude(v => v.Images)
                .ToListAsync();
        }

        public async Task<Product?> GetWithDetailsAsync(Guid id)
        {
            return await _dbContextSet
                .Include(p => p.Images)
                .Include(p => p.Variants)
                    .ThenInclude(v => v.Images)
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }
        public async Task<Product> GetProductDetailsByIdAsync(Guid id)
        {
            return await _dbContext.Products
                .Include(p => p.Variants)
                    .ThenInclude(v => v.Images) // This loads the images for every variant
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }
    }
}
