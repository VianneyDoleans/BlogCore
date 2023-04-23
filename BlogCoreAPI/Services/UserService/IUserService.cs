using System.Collections.Generic;
using System.Threading.Tasks;
using BlogCoreAPI.Models.DTOs.Account;
using BlogCoreAPI.Models.DTOs.Role;
using BlogCoreAPI.Models.DTOs.User;
using DBAccess.Data;
using DBAccess.Specifications;
using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.SortSpecification;

namespace BlogCoreAPI.Services.UserService
{
    public interface IUserService
    {
        Task<IEnumerable<GetAccountDto>> GetAllAccounts();

        public Task<IEnumerable<GetUserDto>> GetUsers(FilterSpecification<User> filterSpecification = null,
            PagingSpecification pagingSpecification = null,
            SortSpecification<User> sortSpecification = null);

        public Task<int> CountUsersWhere(FilterSpecification<User> filterSpecification = null);

        Task<GetAccountDto> GetAccount(int id);

        Task<GetUserDto> GetUser(int id);

        Task<GetAccountDto> GetAccount(string userName);

        Task<User> GetUserEntity(int id);

        Task<GetAccountDto> AddAccount(AddAccountDto account);

        Task AddUserRole(UserRoleDto userRole);

        Task RemoveUserRole(UserRoleDto userRole);

        Task<bool> SignIn(AccountLoginDto accountLogin);

        Task UpdateAccount(UpdateAccountDto account);

        Task DeleteAccount(int id);

        Task<IEnumerable<GetUserDto>> GetUsersFromRole(int id);
        
        Task<IEnumerable<GetRoleDto>> GetDefaultRolesAssignedToNewUsers();
        
        Task SetDefaultRolesAssignedToNewUsers(List<int> roleIds);
    }
}
