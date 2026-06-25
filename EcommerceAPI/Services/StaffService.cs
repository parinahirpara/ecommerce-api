using EcommerceAPI.Dto;
using EcommerceAPI.Interfaces.Repositories;
using EcommerceAPI.Interfaces.Services;
using EcommerceAPI.Models;

namespace EcommerceAPI.Services
{
    public class StaffService : IStaffService
    {
        private readonly IStaffRepository _staffRepository;
        private readonly IAuthService _authService;
        public StaffService(IStaffRepository staffRepository, IAuthService authService)
        {
            _staffRepository = staffRepository;
            _authService = authService;
        }
        public async Task<bool> RegisterAsync(RegisterDto dto)
        {
            Staff? existing = await _staffRepository.GetByEmailAsync(dto.Email);
            if (existing != null)
            {
                return false;
            }

            var hashed = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            Staff? staff = new Staff
            {
                Email = dto.Email,
                PasswordHash = hashed,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                CreatedAt = DateTime.UtcNow
            };

            await _staffRepository.AddAsync(staff);
            await _staffRepository.SaveChangesAsync();

            return true;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            Staff? user = await _staffRepository.GetByEmailAsync(dto.Email);
            {
                if (user == null)
                {
                    return null;
                }

                bool verified = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
                if (!verified)
                {
                    return null;
                }

                string? token = _authService.GenerateToken(user.Id, user.Email);
                DateTime expiry = _authService.GetExpiry();

                return new AuthResponseDto(token, expiry);
            }
        }

        public Task<Staff?> GetByEmailAsync(string email)
        {
            return _staffRepository.GetByEmailAsync(email);
        }
    }
}
