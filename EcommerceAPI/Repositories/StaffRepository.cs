using EcommerceAPI.Data;
using EcommerceAPI.Interfaces.Repositories;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;


namespace EcommerceAPI.Repositories
{
    public class StaffRepository : Repository<Staff>, IStaffRepository
    {
        public StaffRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<Staff?> GetByEmailAsync(string email)
        {
            return await _dbContext.Staffs.FirstOrDefaultAsync(s => s.Email == email);
        }
    }
}
