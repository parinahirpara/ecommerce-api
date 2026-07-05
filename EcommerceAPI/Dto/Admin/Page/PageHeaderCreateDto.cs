namespace EcommerceAPI.Dto.Admin.Page
{
    public class PageHeaderCreateDto
    {
        public string PageKey { get; set; } = string.Empty;
        public string PageTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string LinksMetadataJson { get; set; } = string.Empty;
        public List<IFormFile>? LinkFiles { get; set; }
    }
}
