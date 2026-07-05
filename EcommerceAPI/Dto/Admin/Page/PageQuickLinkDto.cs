namespace EcommerceAPI.Dto.Admin.Page
{
    public class PageQuickLinkDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Route { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }
}
