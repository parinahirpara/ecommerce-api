using EcommerceAPI.Dto;
using EcommerceAPI.Models;

namespace EcommerceAPI.Interfaces.Services
{
    public interface IStaffService
    {
        Task<bool> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
        Task<Staff?> GetByEmailAsync(string email);
    }
}
