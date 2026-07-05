using System.Text.Json.Serialization;

namespace EcommerceAPI.Dto.Customer.Navigation
{
    public class RouteDto
    {
        [JsonPropertyName("path")]
        public string Path { get; set; } = string.Empty;

        [JsonPropertyName("component")]
        public string Component { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public RouteDataDto? Data { get; set; }
    }
}
