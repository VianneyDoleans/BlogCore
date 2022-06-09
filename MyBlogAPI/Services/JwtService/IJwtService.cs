using System.Threading.Tasks;

namespace MyBlogAPI.Services.JwtService
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
        Task<string> GenerateJwt(int userId);
    }
}
