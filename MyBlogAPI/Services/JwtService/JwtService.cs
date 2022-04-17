using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DbAccess.Data.POCO.Jwt;
using DbAccess.Repositories.Role;
using DbAccess.Repositories.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MyBlogAPI.Services.JwtService
{
    public class JwtService : IJwtService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly JwtSettings _jwtSettings;

        public JwtService(IUserRepository userRepository, IRoleRepository roleRepository, IOptions<JwtSettings> jwtSettings)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _jwtSettings = jwtSettings.Value;
        }

        /// <inheritdoc />
        public async Task<string> GenerateJwt(int userId)
        {
            var user = await _userRepository.GetAsync(userId);
            var roles = await _roleRepository.GetRolesFromUser(userId);
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r.Name));
            claims.AddRange(roleClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_jwtSettings.ExpirationInDays));

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Issuer,
                claims,
                expires: expires,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
