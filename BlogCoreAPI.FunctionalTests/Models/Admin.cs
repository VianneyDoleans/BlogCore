using System.Text.Json.Serialization;

namespace BlogCoreAPI.FunctionalTests.Models
{
    public class Admin
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
