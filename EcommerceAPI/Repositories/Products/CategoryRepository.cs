using EcommerceAPI.Data;
using EcommerceAPI.Dto.Customer.Navigation;
using EcommerceAPI.Interfaces.Repositories.Products;
using EcommerceAPI.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Repositories.Products
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        // Custom method to handle complex Eager Loading safely
        public async Task<IEnumerable<Category>> GetAllWithSubCategoriesAsync()
        {
            return await _context.Categories
                .Include(c => c.SubCategories)
                .ToListAsync();
        }
        public async Task<(List<Category> Categories, List<Product> Products)> GetMenuConfigurationDataAsync()
        {
            var categories = await _context.Categories
            .Where(c => c.IsActive)
            .Include(c => c.SubCategories.Where(s => s.IsActive))
            .ToListAsync();

                var categoryIds = categories.Select(c => c.Id).ToList();

                // 2. Query products directly via CategoryId foreign key mapping
                var products = await _context.Products
                    .Where(p => p.IsActive && categoryIds.Contains(p.CategoryId))
                    .Include(p => p.Variants)
                        .ThenInclude(v => v.Material)
                    .ToListAsync();

                return (categories, products);
            }
    }
}
