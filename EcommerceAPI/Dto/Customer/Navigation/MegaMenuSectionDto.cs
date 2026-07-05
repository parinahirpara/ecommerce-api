using System.Text.Json.Serialization;

namespace EcommerceAPI.Dto.Customer.Navigation
{
    public class MegaMenuSectionDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("items")]
        public List<MenuItemDto> Items { get; set; }
    }
}
