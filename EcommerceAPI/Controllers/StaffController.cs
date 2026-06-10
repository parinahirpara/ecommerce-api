using EcommerceAPI.Dto;
using EcommerceAPI.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;

        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var success = await _staffService.RegisterAsync(dto);
            if (!success)
                return Conflict(new { message = "Email already registered" });

            return Ok(new { message = "Registration successful" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            AuthResponseDto? auth = await _staffService.LoginAsync(dto);
            if (auth == null) return Unauthorized("Invalid credentials.");
            return Ok(auth);
        }
    }
}
