namespace EcommerceAPI.Dto.Products
{
    public class ProductVariantDto
    {
        public Guid ProductId { get; set; }
        public string VariantSKU { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public decimal Weight { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPercentage { get; set; }
        public int StockQuantity { get; set; }
        public List<IFormFile>? Files {get; set; }

        public string ImagesMetadata { get; set; } = string.Empty;

    }
}
