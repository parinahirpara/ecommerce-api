using EcommerceAPI.Models.Page;

namespace EcommerceAPI.Interfaces.Repositories.Page
{
    public interface IPageQuickLinkRepository : IRepository<PageQuickLink>
    {
        Task<IEnumerable<PageQuickLink>> GetLinksByHeaderIdAsync(Guid pageHeaderId);
    }
}
