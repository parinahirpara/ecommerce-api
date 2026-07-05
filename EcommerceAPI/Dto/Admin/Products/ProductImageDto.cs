namespace EcommerceAPI.Dto.Admin.Products
{
    public class ProductImageDto
    {
        public Guid Id { get; set; } 
        public string ImageUrl { get; set; } = string.Empty;
        public string AltText { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }
}
