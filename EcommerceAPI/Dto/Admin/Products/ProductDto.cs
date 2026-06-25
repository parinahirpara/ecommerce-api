namespace EcommerceAPI.Dto.Admin.Products
{
    public class ProductDto
    {
        public Guid? Id { get; set; }
        public string ProductSKU { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public string ProductShortDescription { get; set; } = string.Empty;
        public string ProductBrand { get; set; } = string.Empty;
        public Guid CategoryId { get; set; } 
        public Guid SubCategoryId { get; set; } 
        public bool IsActive { get; set; } = true;

        public string CategoryName { get; set; } = string.Empty;    
        public string SubCategoryName { get; set; } = string.Empty; 

        public List<ProductVariantResponseDto> Variants { get; set; } = new List<ProductVariantResponseDto>();
    }
}
