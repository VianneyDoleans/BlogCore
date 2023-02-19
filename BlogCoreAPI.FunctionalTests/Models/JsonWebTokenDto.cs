using System.Text.Json.Serialization;

namespace BlogCoreAPI.FunctionalTests.Models
{
    public class JsonWebTokenDto
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
