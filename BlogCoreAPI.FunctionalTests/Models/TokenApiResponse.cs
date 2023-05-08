using System;
using System.Text.Json.Serialization;

namespace BlogCoreAPI.FunctionalTests.Models
{
    public class TokenApiResponse
    {
        [JsonPropertyName("userName")]
        public string UserName { get; set; }
    
        [JsonPropertyName("userId")]
        public int UserId { get; set; }
    
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }
    
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    
        [JsonPropertyName("accessTokenExpiration")]
        public DateTime AccessTokenExpiration { get; set; }
    
        [JsonPropertyName("refreshTokenExpiration")]
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
