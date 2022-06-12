using System.Collections.Generic;
using System.Threading.Tasks;
using DbAccess.Data.POCO;
using DbAccess.Specifications;
using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.SortSpecification;
using MyBlogAPI.DTOs.User;

namespace MyBlogAPI.Services.UserService
{
    public interface IUserService
    {
        Task<IEnumerable<GetUserDto>> GetAllUsers();

        public Task<IEnumerable<GetUserDto>> GetUsers(FilterSpecification<User> filterSpecification = null,
            PagingSpecification pagingSpecification = null,
            SortSpecification<User> sortSpecification = null);

        public Task<int> CountUsersWhere(FilterSpecification<User> filterSpecification = null);

        Task<GetUserDto> GetUser(int id);

        Task<GetUserDto> GetUser(string userName);

        Task<User> GetUserEntity(int id);

        Task<GetUserDto> AddUser(AddUserDto user);

        Task AddUserRole(UserRoleDto userRole);

        Task RemoveUserRole(UserRoleDto userRole);

        Task<bool> SignIn(UserLoginDto userLogin);

        Task UpdateUser(UpdateUserDto user);

        Task DeleteUser(int id);

        Task<IEnumerable<GetUserDto>> GetUsersFromRole(int id);
    }
}
