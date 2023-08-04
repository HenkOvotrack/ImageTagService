using System.Text.Json.Serialization;

namespace Ovotrack4.ImageTag.Service.Models
{
    public class ImageTagPushMessage
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime? Timestamp { get; set; }

        [JsonPropertyName("action")]
        public string? Action { get; set; }

        [JsonPropertyName("target")]
        public Target? Target { get; set; }

        [JsonPropertyName("request")]
        public Request? Request { get; set; }
    }
    public class Target
    {
        [JsonPropertyName("mediaType")]
        public string? MediaType { get; set; }

        [JsonPropertyName("size")]
        public int? Size { get; set; }

        [JsonPropertyName("digest")]
        public string? Digest { get; set; }

        [JsonPropertyName("length")]
        public int? Length { get; set; }

        [JsonPropertyName("repository")]
        public string? Repository { get; set; }

        [JsonPropertyName("tag")]
        public string? Tag { get; set; }
    }
    public class Request
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("host")]
        public string? Host { get; set; }

        [JsonPropertyName("method")]
        public string? Method { get; set; }

        [JsonPropertyName("useragent")]
        public string? Useragent { get; set; }
    }
}
