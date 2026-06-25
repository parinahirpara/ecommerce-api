namespace EcommerceAPI.Dto.Admin.Products
{
    public class MaterialDto
    {
        public Guid Id { get; set; } 
        public string MaterialName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
