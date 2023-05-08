using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BlogCoreAPI.Models.DTOs;
using BlogCoreAPI.Models.DTOs.Immutable;
using DBAccess.Repositories.UnitOfWork;
using DBAccess.Repositories.User;
using DBAccess.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BlogCoreAPI.Services.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenSettings _tokenSettings;
        private readonly IUnitOfWork _unitOfWork;

        public TokenService(IUserRepository userRepository, IUnitOfWork unitOfWork, 
            IOptions<TokenSettings> jwtSettings)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _tokenSettings = jwtSettings.Value;
        }

        /// <inheritdoc />
        public async Task<TokenItem> GenerateJwtAccessToken(int userId)
        {
            var user = await _userRepository.GetAsync(userId);
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTimeOffset.UtcNow.UtcDateTime.AddMinutes(Convert.ToDouble(_tokenSettings.AccessTokenExpirationInMinutes));

            var token = new JwtSecurityToken(
                issuer: _tokenSettings.Issuer,
                audience: _tokenSettings.Issuer,
                claims,
                expires: expires,
                signingCredentials: credentials
            );

            return new TokenItem(new JwtSecurityTokenHandler().WriteToken(token), user.UserName, user.Id, expires);
        }
        
        /// <inheritdoc />
        public async Task<TokenItem> GenerateRefreshToken(int userId)
        {
            var user = await _userRepository.GetAsync(userId);
            
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var refreshToken = Convert.ToBase64String(randomNumber);
            var expires = DateTimeOffset.UtcNow.UtcDateTime.AddMinutes(Convert.ToDouble(_tokenSettings.RefreshTokenExpirationInMinutes));

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiration = expires;
            _unitOfWork.Save();
            
            return new TokenItem(refreshToken, user.UserName, user.Id, expires);
        }
        
        public async Task<bool> IsAuthenticAndValidRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var user = await _userRepository.GetAsync(refreshTokenDto.UserId);

            return user.RefreshToken != null && user.RefreshToken == refreshTokenDto.RefreshToken && user.RefreshTokenExpiration > DateTimeOffset.UtcNow.UtcDateTime;
        }
    }
}
