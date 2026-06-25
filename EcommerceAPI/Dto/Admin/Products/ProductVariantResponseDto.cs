namespace EcommerceAPI.Dto.Admin.Products
{
    public class ProductVariantResponseDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductSKU { get; set; } = string.Empty;
        public string VariantSKU { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public Guid MaterialId { get; set; }
        public string MaterialName { get; set; } = string.Empty;
        public decimal Weight { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPercentage { get; set; }
        public int StockQuantity { get; set; }
        public List<ProductImageDto> Images { get; set; } = new List<ProductImageDto>();
    }
}
