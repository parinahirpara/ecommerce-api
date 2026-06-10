using EcommerceAPI.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EcommerceAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly byte[] _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expiresInMinutes;

        public AuthService(IConfiguration config)
        {
            _config = config;
            _key = Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? throw new ArgumentNullException("Jwt key missing"));
            _issuer = _config["Jwt:Issuer"] ?? "issuer";
            _audience = _config["Jwt:Audience"] ?? "audience";
            _expiresInMinutes = int.TryParse(_config["Jwt:ExpiresInMinutes"], out var m) ? m : 60;
        }
        public string GenerateToken(Guid userId, string email)
        {
            var expiresAt = DateTime.UtcNow.AddMinutes(_expiresInMinutes);

            // Minimal claims: sub (subject) + jti; avoid roles/custom claims
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var creds = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiresAt,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public DateTime GetExpiry()
        {
            return DateTime.UtcNow.AddMinutes(_expiresInMinutes);
        }
    }
}
