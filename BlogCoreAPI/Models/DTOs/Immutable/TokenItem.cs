using System;

namespace BlogCoreAPI.Models.DTOs.Immutable
{
    public class TokenItem
    {
        public string Token { get; }
        
        public DateTimeOffset TokenExpiration { get; }

        public string UserName { get; }

        public int UserId { get; }

        public TokenItem(string token, string userName, int userId, DateTimeOffset tokenExpiration)
        {
            Token = token;
            UserName = userName;
            UserId = userId;
            TokenExpiration = tokenExpiration;
        }
    }
}
