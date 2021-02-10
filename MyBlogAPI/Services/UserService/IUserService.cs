using System.Collections.Generic;
using System.Threading.Tasks;
using MyBlogAPI.DTO.User;

namespace MyBlogAPI.Services.UserService
{
    public interface IUserService
    {
        Task<IEnumerable<GetUserDto>> GetAllUsers();

        Task<GetUserDto> GetUser(int id);

        Task AddUser(AddUserDto user);

        Task UpdateUser(AddUserDto user);

        Task DeleteUser(int id);
    }
}
