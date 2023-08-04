using System.Text.Json.Serialization;

namespace Ovotrack4.ImageTag.Service.Models
{
    public class ImageVariableTagDto
    {
        [JsonPropertyName("imageName")]
        public string? imageName { get; set; }

        [JsonPropertyName("variableName")]
        public string? variableName { get; set; }
        [JsonPropertyName("latestVersionTag")]
        public string? latestVersionTag { get; set; }
    }
}
