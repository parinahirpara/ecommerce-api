namespace EcommerceAPI.Dto.Admin.Page
{
    public class PageQuickLinkMetadataDto
    {
        public string? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Route { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public string? ImageUrl { get; set; }
    }
}
