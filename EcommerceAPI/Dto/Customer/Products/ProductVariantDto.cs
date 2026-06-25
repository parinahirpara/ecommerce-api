namespace EcommerceAPI.Dto.Customer.Products
{
    public class ProductVariantDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string VariantSKU { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public decimal Weight { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPercentage { get; set; }
        public int StockQuantity { get; set; }

        public Guid MaterialId { get; set; }
        public string MaterialName { get; set; } = string.Empty;

        public List<ProductImageDto> Images { get; set; } = new List<ProductImageDto>();
    }
}
