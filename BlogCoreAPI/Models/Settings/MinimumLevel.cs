using System.Text.Json.Serialization;

namespace BlogCoreAPI.Models.Settings
{
    public class MinimumLevel
    {
        [JsonPropertyName("Default")]
        public string Default { get; set; }
    }
}
