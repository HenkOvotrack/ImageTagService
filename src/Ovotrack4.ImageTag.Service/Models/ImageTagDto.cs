using System.Text.Json.Serialization;

namespace Ovotrack4.ImageTag.Service.Models
{
    public class ImageTagDto
    {


        [JsonPropertyName("id")]
        public string? id { get; set; }
        [JsonPropertyName("repository")]
        public string? repository { get; set; }
        [JsonPropertyName("tag")]
        public string? tag { get; set; }
        
    }
}
