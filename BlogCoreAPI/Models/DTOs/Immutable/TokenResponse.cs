using System;

namespace BlogCoreAPI.Models.DTOs.Immutable;

public class TokenResponse
{
    
    public string UserName { get; }
    
    public int UserId { get; }
    
    public string AccessToken { get; }
    
    public string RefreshToken { get; }
    
    public DateTimeOffset AccessTokenExpiration { get; }
    
    public DateTimeOffset RefreshTokenExpiration { get; }

    public TokenResponse(TokenItem accessToken, TokenItem refreshToken)
    {
        UserName = refreshToken.UserName;
        UserId = refreshToken.UserId;
        RefreshToken = refreshToken.Token;
        RefreshTokenExpiration = refreshToken.TokenExpiration;
        AccessToken = accessToken.Token;
        AccessTokenExpiration = accessToken.TokenExpiration;
    }
}