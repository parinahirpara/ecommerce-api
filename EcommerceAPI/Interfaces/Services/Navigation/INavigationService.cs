using EcommerceAPI.Dto.Customer.Navigation;

namespace EcommerceAPI.Interfaces.Services.Navigation
{
    public interface INavigationService
    {
        Task<List<MenuResponseDto>> GetMenuConfigurationAsync();
    }
}
