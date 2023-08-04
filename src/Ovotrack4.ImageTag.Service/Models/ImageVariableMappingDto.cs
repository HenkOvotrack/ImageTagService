using System.Text.Json.Serialization;

namespace Ovotrack4.ImageTag.Service.Models
{
    public class ImageVariableMappingDto
    {
        [JsonPropertyName("id")]
        public string? id { get; set; }
        [JsonPropertyName("variableName")]
        public string? variableName { get; set; }
        [JsonPropertyName("imageName")]
        public string? imageName { get; set; }
    }
}
