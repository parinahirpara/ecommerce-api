namespace EcommerceAPI.Models.Products
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public string ProductSKU { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty; // Admin side
        public string ProductTitle { get; set; } = string.Empty; // Client side
        public string ProductDescription { get; set; } = string.Empty;
        public string ProductShortDescription { get; set; } = string.Empty;
        public string ProductBrand { get; set; } = string.Empty;
        public string CategoryId { get; set; } = string.Empty; // Rings, bracelet, charm, necklace
        public string SubCategoryId { get; set; } = string.Empty;  //promise ring,engagement ring etc
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
        public virtual ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    }
}
