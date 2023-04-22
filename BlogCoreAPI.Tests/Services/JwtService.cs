using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogCoreAPI.Services.JwtService;
using BlogCoreAPI.Services.UrlService;
using BlogCoreAPI.Tests.Builders;
using BlogCoreAPI.Validators.User;
using DBAccess.Repositories.Role;
using DBAccess.Repositories.User;
using DBAccess.Settings;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace BlogCoreAPI.Tests.Services
{
    public class JwtService : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly string _issuer;
        private readonly int _expirationInDays;

        public JwtService(DatabaseFixture databaseFixture)
        {
            _fixture = databaseFixture;
            _issuer = "test321";
            _expirationInDays = 1;
            var config = new MapperConfiguration(cfg => { cfg.AddProfile(databaseFixture.MapperProfile); });
            _mapper = config.CreateMapper();
            _jwtService = new BlogCoreAPI.Services.JwtService.JwtService(
                new UserRepository(_fixture.Db, _fixture.UserManager), Options.Create(
                    new JwtSettings() { 
                        Issuer = _issuer, 
                        Secret = "test123ABDZDAZSQA", 
                        ExpirationInDays = _expirationInDays
                    }));
        }

        [Fact]
        public async Task GenerateJwt()
        {
            var userService = new BlogCoreAPI.Services.UserService.UserService(new UserRepository(_fixture.Db, _fixture.UserManager), new RoleRepository(_fixture.Db, _fixture.RoleManager),
                _mapper, _fixture.UnitOfWork, new UserDtoValidator(Mock.Of<IUrlService>()));
            var account = await new AccountBuilder(userService).Build();

            // Act
            var token = (await _jwtService.GenerateJwt(account.Id)).Token;

            // Assert
            Assert.NotNull(token);
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            Assert.Equal(_issuer, jwtSecurityToken.Issuer);
            Assert.Contains(jwtSecurityToken.Claims, x =>
                x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier" &&
                x.Value == account.Id.ToString());
            var jwtExpValue = long.Parse(jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "exp")?.Value ?? "0");
            var expirationTime = DateTimeOffset.FromUnixTimeSeconds(jwtExpValue).DateTime;
            Assert.Equal(expirationTime, DateTime.UtcNow.AddDays(_expirationInDays), TimeSpan.FromSeconds(5));
        }
    }
}
