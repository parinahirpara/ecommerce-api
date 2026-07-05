using System.Text.Json.Serialization;

namespace EcommerceAPI.Dto.Customer.Navigation
{
    public class MenuResponseDto
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("route")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Route { get; set; }

        [JsonPropertyName("megaMenu")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<MegaMenuSectionDto> MegaMenu { get; set; }
    }
}
