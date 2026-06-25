using EcommerceAPI.Data;
using EcommerceAPI.Dto.Common;
using EcommerceAPI.Interfaces.Repositories.Products;
using EcommerceAPI.Models.Products;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<Product> GetProductDetailsByIdAsync(Guid id)
        {
            return await _dbContext.Products
                .Include(p => p.Variants)
                    .ThenInclude(v => v.Images)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PagedResult<Product>> GetPagedProductWithDetailsAsync(int skip, int take, Guid? categoryId, Guid? subCategoryId)
        {
            var baseQuery = _dbContext.Products.Where(p => p.IsActive);

            if (categoryId.HasValue && categoryId.Value != Guid.Empty)
            {
                baseQuery = baseQuery.Where(p => p.CategoryId == categoryId.Value);
            }

            if (subCategoryId.HasValue && subCategoryId.Value != Guid.Empty)
            {
                baseQuery = baseQuery.Where(p => p.SubCategoryId == subCategoryId.Value);
            }

            int totalCount = await baseQuery.CountAsync();

            // 3. Complete Eager Loading Graph
            var items = await baseQuery
                .Include(p => p.Category)
                .Include(p => p.SubCategory)
                .Include(p => p.Variants)
                    .ThenInclude(v => v.Material)
                .Include(p => p.Variants)
                    .ThenInclude(v => v.Images)
                .OrderBy(p => p.CreatedDate)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return new PagedResult<Product>(items, totalCount, skip);
        }
    }
}
