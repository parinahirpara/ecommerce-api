using EcommerceAPI.Data;
using EcommerceAPI.Interfaces.Repositories.Products;
using EcommerceAPI.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Repositories.Products
{
    public class SubCategoryRepository : Repository<SubCategory>, ISubCategoryRepository
    {
        public SubCategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        // Fetch subcategories filtered by a specific parent ID
        public async Task<IEnumerable<SubCategory>> GetSubCategoriesByCategoryIdAsync(int categoryId)
        {
            return await _dbContextSet
                .Where(s => s.CategoryId == categoryId)
                .Include(s => s.Category)
                .ToListAsync();
        }

        // Fetch all records with full parent object context
        public async Task<IEnumerable<SubCategory>> GetAllWithCategoryAsync()
        {
            return await _dbContextSet
                .Include(s => s.Category)
                .ToListAsync();
        }
    }
}
