using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using DbAccess.Data.POCO;
using DbAccess.Repositories.Role;
using DbAccess.Repositories.UnitOfWork;
using DbAccess.Repositories.User;
using DbAccess.Specifications;
using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.FilterSpecifications.Filters;
using DbAccess.Specifications.SortSpecification;
using MyBlogAPI.DTOs.User;

namespace MyBlogAPI.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository repository, IRoleRepository roleRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GetUserDto>> GetAllUsers()
        {
            var users = await _repository.GetAllAsync();
            return users.Select(c =>
            {
               var usersDto = _mapper.Map<GetUserDto>(c);
               usersDto.Roles = c.UserRoles.Select(x => x.RoleId);
               return usersDto;
            }).ToList();
        }

        public async Task<IEnumerable<GetUserDto>> GetUsers(FilterSpecification<User> filter = null, PagingSpecification paging = null,
            SortSpecification<User> sort = null)
        {
            return (await _repository.GetAsync(filter, paging, sort)).Select(x => _mapper.Map<GetUserDto>(x));
        }

        public async Task<int> CountUsersWhere(FilterSpecification<User> filterSpecification = null)
        {
            return await _repository.CountWhereAsync(filterSpecification);
        }

        public async Task<GetUserDto> GetUser(int id)
        {
            var user = await _repository.GetAsync(id);
                var userDto = _mapper.Map<GetUserDto>(user);
                userDto.Roles = user.UserRoles.Select(x => x.RoleId);
                return userDto;
        }

        public async Task<User> GetUserEntity(int id)
        {
            return await _repository.GetAsync(id);
        }

        private async Task<User> FindUser(string userName)
        {
            var user = (await _repository.GetAsync(new UsernameSpecification<User>(userName))).ToList();

            if (user == null || user.Count() != 1)
            {
                throw new IndexOutOfRangeException("User doesn't exists.");
            }
            return user.First();
        }

        public async Task<GetUserDto> GetUser(string userName)
        {
            var user = await FindUser(userName);
            var userDto = _mapper.Map<GetUserDto>(user);
            userDto.Roles = user.UserRoles.Select(x => x.RoleId);
            return userDto;
        }

        private static void CheckUsernameValidity(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("UserName cannot be null or empty.");
            if (!Regex.Match(username, @"^(?!.*[._()\[\]-]{2})[A-Za-z0-9._()\[\]-]{3,20}$").Success)
                throw new ArgumentException("UserName must consist of between 3 to 20 allowed characters (A-Z, a-z, 0-9, .-_()[]) and cannot contain two consecutive symbols.");
        }

        private static void CheckEmailAddressValidity(string emailAddress)
        {
            if (string.IsNullOrWhiteSpace(emailAddress))
                throw new ArgumentException("Email cannot be null or empty.");
            if (emailAddress.Length > 320)
                throw new ArgumentException("Email address cannot exceed 320 characters.");
            if (!Regex.Match(emailAddress,
                    @"\A[a-zA-Z0-9.!\#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*\z")
                .Success)
                throw new ArgumentException("Email address is invalid.");
        }

        private static void CheckPasswordValidity(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be null or empty.");
        }

        private static void CheckUserDescription(string userDescription)
        {
            if (userDescription != null && userDescription.Length > 1000)
                throw new ArgumentException("User description cannot exceed 1000 characters.");
        }

        private static void CheckUserValidity(IUserDto user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            CheckEmailAddressValidity(user.Email);
            CheckUsernameValidity(user.UserName);
            CheckPasswordValidity(user.Password);
            CheckUserDescription(user.UserDescription);
        }

        private async Task CheckUserValidity(AddUserDto user)
        {
            CheckUserValidity((IUserDto) user);
            if (await _repository.UserNameAlreadyExists(user.UserName))
                throw new InvalidOperationException("UserName already exists.");
            if (await _repository.EmailAlreadyExists(user.Email))
                throw new InvalidOperationException("Email Address already exists.");
        }

        private async Task CheckUserValidity(UpdateUserDto user)
        {
            var userDb = _repository.GetAsync(user.Id);
            CheckUserValidity((IUserDto)user);
            if (await _repository.UserNameAlreadyExists(user.UserName) &&
                (await userDb).UserName != user.UserName)
                throw new InvalidOperationException("UserName already exists.");
            if (await _repository.EmailAlreadyExists(user.Email) &&
                (await userDb).Email != user.Email)
                throw new InvalidOperationException("Email Address already exists.");
        }

        public async Task<GetUserDto> AddUser(AddUserDto user)
        {
            await CheckUserValidity(user);
            var result = await _repository.AddAsync(_mapper.Map<User>(user));
           _unitOfWork.Save();
           return _mapper.Map<GetUserDto>(result);
        }

        public async Task AddUserRole(UserRoleDto userRole)
        {
            var user = (await _repository.GetAsync(userRole.UserId));
            if (user == null)
            {
                throw new IndexOutOfRangeException("User doesn't exists.");
            }

            var role = (await _roleRepository.GetAsync(userRole.RoleId));
            if (role == null)
            {
                throw new IndexOutOfRangeException("Role doesn't exists.");
            }

            if (user.UserRoles.Any(x => x.UserId == userRole.UserId && x.RoleId == userRole.RoleId))
                throw new InvalidOperationException("User already have the role.");
            await _repository.AddRoleToUser(user, role);
            _unitOfWork.Save();
        }

        public async Task RemoveUserRole(UserRoleDto userRole)
        {
            var user = (await _repository.GetAsync(userRole.UserId));
            if (user == null)
            {
                throw new IndexOutOfRangeException("User doesn't exists.");
            }

            var role = (await _roleRepository.GetAsync(userRole.RoleId));
            if (role == null)
            {
                throw new IndexOutOfRangeException("Role doesn't exists.");
            }

            if (!user.UserRoles.Any(x => x.UserId == userRole.UserId && x.RoleId == userRole.RoleId))
                throw new InvalidOperationException("User doesn't have the role.");
            await _repository.RemoveRoleToUser(user, role);
            _unitOfWork.Save();
        }

        public async Task<bool> SignIn(UserLoginDto userLogin)
        {
            var user = await FindUser(userLogin.UserName);

            var isValidPassword = await _repository.CheckPasswordAsync(user, userLogin.Password);
            return isValidPassword;
        }

        public async Task UpdateUser(UpdateUserDto user)
        {
            if (await UserAlreadyExistsWithSameProperties(user))
                return;
            await CheckUserValidity(user);
            var userEntity = await _repository.GetAsync(user.Id);
            _mapper.Map(user, userEntity);
            _unitOfWork.Save();
        }

        public async Task DeleteUser(int id)
        {
            await _repository.RemoveAsync(await _repository.GetAsync(id));
            _unitOfWork.Save();
        }

        public async Task<IEnumerable<GetUserDto>> GetUsersFromRole(int id)
        {
            var users = await _repository.GetUsersFromRole(id);
            var usersDto = users.Select(x =>
            {
                var userDto = _mapper.Map<GetUserDto>(x);
                userDto.Roles = x.UserRoles.Select(y => y.RoleId);
                return userDto;
            }).ToList();
            return usersDto;
        }

        private async Task<bool> UserAlreadyExistsWithSameProperties(UpdateUserDto user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            var userDb = await _repository.GetAsync(user.Id);
            return userDb.UserName == user.UserName &&
                   userDb.Email == user.Email &&
                   user.UserDescription == userDb.UserDescription;
        }
    }
}
