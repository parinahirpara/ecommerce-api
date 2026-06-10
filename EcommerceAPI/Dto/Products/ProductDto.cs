namespace EcommerceAPI.Dto.Products
{
    public class ProductDto
    {
        public Guid? ProductId { get; set; }
        public string ProductSKU { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string ProductTitle { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public string ProductShortDescription { get; set; } = string.Empty;
        public string ProductBrand { get; set; } = string.Empty;
        public string CategoryId { get; set; } = string.Empty;
        public string SubCategoryId { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
