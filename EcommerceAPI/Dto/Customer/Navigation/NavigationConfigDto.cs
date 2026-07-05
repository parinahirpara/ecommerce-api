using System.Text.Json.Serialization;

namespace EcommerceAPI.Dto.Customer.Navigation
{
    public class NavigationConfigDto
    {
        [JsonPropertyName("menu")]
        public List<MenuResponseDto> Menu { get; set; } = new();

        [JsonPropertyName("routes")]
        public List<RouteDto> Routes { get; set; } = new();
    }
}