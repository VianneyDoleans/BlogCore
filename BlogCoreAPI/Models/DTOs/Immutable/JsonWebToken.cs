namespace BlogCoreAPI.Models.DTOs.Immutable
{
    public class JsonWebToken
    {
        public string Token { get; }

        public JsonWebToken(string token)
        {
            Token = token;
        }
    }
}
