using System.Text.Json.Serialization;

namespace BlogCoreAPI.Models.Settings
{
    public class SerilogSettings
    {
        [JsonPropertyName("MinimumLevel")]
        public MinimumLevel MinimumLevel { get; set; }

        public const string Position = "Serilog";
    }
}