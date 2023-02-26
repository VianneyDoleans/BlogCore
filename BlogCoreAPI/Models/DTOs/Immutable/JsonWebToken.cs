namespace BlogCoreAPI.Models.DTOs.Immutable
{
    public class JsonWebToken
    {
        public string Token { get; }

        public string UserName { get; }

        public int UserId { get; }

        public JsonWebToken(string token, string userName, int userId)
        {
            Token = token;
            UserName = userName;
            UserId = userId;
        }
    }
}
