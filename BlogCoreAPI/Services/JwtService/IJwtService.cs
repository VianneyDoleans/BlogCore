using System.Threading.Tasks;
using BlogCoreAPI.Models.DTOs.Immutable;

namespace BlogCoreAPI.Services.JwtService
{
    /// <summary>
    /// Service used to manipulate JWT
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        /// Generate a unique JWT for a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<JsonWebToken> GenerateJwt(int userId);
    }
}
