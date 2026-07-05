using EcommerceAPI.Dto.Admin.Page;

namespace EcommerceAPI.Interfaces.Services.Page
{
    public interface IPageHeaderService
    {
        Task<IEnumerable<PageHeaderDto>> GetAllPageHeadersAsync();
        Task<PageHeaderDto?> GetPageHeaderAsync(string pageKey);
        Task<PageHeaderDto> CreatePageHeaderAsync(PageHeaderCreateDto dto);
        Task<PageHeaderDto> UpdatePageHeaderAsync(Guid pageHeaderId, PageHeaderCreateDto dto);
    }
}
