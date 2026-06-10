using EcommerceAPI.Models;

namespace EcommerceAPI.Interfaces.Repositories
{
    public interface IStaffRepository : IRepository<Staff>
    {
        Task<Staff?> GetByEmailAsync(string email);
    }
}
