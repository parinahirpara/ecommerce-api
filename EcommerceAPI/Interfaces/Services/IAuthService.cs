namespace EcommerceAPI.Interfaces.Services
{
    public interface IAuthService
    {
        string GenerateToken(Guid userId, string email);
        DateTime GetExpiry();
    }
}
