using System.Text.Json.Serialization;

namespace EcommerceAPI.Dto.Customer.Navigation
{
    public class RouteDataDto
    {
        [JsonPropertyName("categoryKey")]
        public string CategoryKey { get; set; } = string.Empty;

        public Guid? CategoryId { get; set; } 

        [JsonPropertyName("material")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Material { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Guid? MaterialId { get; set; } 

        [JsonPropertyName("subCategoryKey")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SubCategoryKey { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Guid? SubCategoryId { get; set; } 
    }
}
