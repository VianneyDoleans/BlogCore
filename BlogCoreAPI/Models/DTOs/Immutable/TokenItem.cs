using System;

namespace BlogCoreAPI.Models.DTOs.Immutable
{
    public class TokenItem
    {
        public string Token { get; }
        
        public DateTime TokenExpiration { get; }

        public string UserName { get; }

        public int UserId { get; }

        public TokenItem(string token, string userName, int userId, DateTime tokenExpiration)
        {
            Token = token;
            UserName = userName;
            UserId = userId;
            TokenExpiration = tokenExpiration;
        }
    }
}
