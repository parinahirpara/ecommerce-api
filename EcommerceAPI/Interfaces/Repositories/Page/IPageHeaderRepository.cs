using EcommerceAPI.Models.Page;

namespace EcommerceAPI.Interfaces.Repositories.Page
{
    public interface IPageHeaderRepository : IRepository<PageHeader>
    {
        Task<IEnumerable<PageHeader>> GetAllPageHeaderAsync();
        Task<PageHeader?> GetByPageKeyWithLinksAsync(string pageKey);
    }
}
