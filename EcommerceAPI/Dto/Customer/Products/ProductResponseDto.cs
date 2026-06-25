namespace EcommerceAPI.Dto.Customer.Products
{
    public class ProductResponseDto
    {
        public Guid Id { get; set; }
        public string ProductSKU { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public string ProductShortDescription { get; set; } = string.Empty;
        public string ProductBrand { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public Guid SubCategoryId { get; set; }
        public string SubCategoryName { get; set; } = string.Empty;
        public List<ProductVariantDto> Variants { get; set; } = new List<ProductVariantDto>();
    }
}
