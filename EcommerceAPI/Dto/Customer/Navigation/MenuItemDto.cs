using System.Text.Json.Serialization;

namespace EcommerceAPI.Dto.Customer.Navigation
{
    public class MenuItemDto
    {
        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("route")]
        public string Route { get; set; }

        // Kept internal for mapping purposes, won't show in frontend JSON serialization
        [JsonIgnore]
        public Guid Id { get; set; }
    }
}
