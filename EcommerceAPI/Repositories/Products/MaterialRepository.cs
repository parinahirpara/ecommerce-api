using EcommerceAPI.Data;
using EcommerceAPI.Interfaces.Repositories.Products;
using EcommerceAPI.Models.Products;

namespace EcommerceAPI.Repositories.Products
{
    public class MaterialRepository : Repository<Material>, IMaterialRepository
    {
        public MaterialRepository(ApplicationDbContext context) : base(context) { }
    }
}
