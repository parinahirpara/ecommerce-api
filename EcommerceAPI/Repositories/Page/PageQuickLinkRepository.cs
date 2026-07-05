using EcommerceAPI.Data;
using EcommerceAPI.Interfaces.Repositories.Page;
using EcommerceAPI.Models.Page;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Repositories.Page
{
    public class PageQuickLinkRepository : Repository<PageQuickLink>, IPageQuickLinkRepository
    {
        public PageQuickLinkRepository(ApplicationDbContext dbContext) : base(dbContext) { }
        public async Task<IEnumerable<PageQuickLink>> GetLinksByHeaderIdAsync(Guid pageHeaderId)
        {
            return await _dbContext.PageQuickLinks
                .Where(link => link.PageHeaderId == pageHeaderId)
                .ToListAsync();
        }

    }
}
