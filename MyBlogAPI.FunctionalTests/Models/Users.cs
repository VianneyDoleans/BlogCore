using System.Text.Json.Serialization;

namespace MyBlogAPI.FunctionalTests.Models
{
    public class Users
    {
        [JsonPropertyName("Admin")]
        public Admin Admin { get; set; }

        [JsonPropertyName("User")]
        public Admin User { get; set; }
    }
}
