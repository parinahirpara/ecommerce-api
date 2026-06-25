namespace EcommerceAPI.Dto.Admin.Products
{
    public class VariantImageMetadataDto
    {
        public Guid? Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
