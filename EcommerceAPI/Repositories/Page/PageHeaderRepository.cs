using EcommerceAPI.Data;
using EcommerceAPI.Interfaces.Repositories.Page;
using EcommerceAPI.Models.Page;
using EcommerceAPI.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Repositories.Page
{
    public class PageHeaderRepository : Repository<PageHeader>, IPageHeaderRepository
    {
        public PageHeaderRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<PageHeader>> GetAllPageHeaderAsync()
        {
            return await _dbContext.PageHeaders
                .Include(p => p.QuickLinks)
                .ToListAsync();
        }

        public async Task<PageHeader?> GetByPageKeyWithLinksAsync(string pageKey)
        {
            return await _dbContext.PageHeaders
                .Include(p => p.QuickLinks)
                .FirstOrDefaultAsync(p => p.PageKey.ToLower() == pageKey.ToLower());
        }
    }
}
