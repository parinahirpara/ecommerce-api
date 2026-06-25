using EcommerceAPI.Data;
using EcommerceAPI.Interfaces.Repositories.Products;
using EcommerceAPI.Models.Products;

namespace EcommerceAPI.Repositories.Products
{
    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    }
}
