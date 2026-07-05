namespace EcommerceAPI.Dto.Admin.Page
{
    public class PageHeaderDto
    {
        public Guid Id { get; set; }
        public string PageKey { get; set; } = string.Empty;
        public string PageTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<PageQuickLinkDto> QuickLinks { get; set; } = new();
    }
}
