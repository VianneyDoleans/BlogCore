using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using DbAccess.Data.POCO;
using DbAccess.Repositories.UnitOfWork;
using DbAccess.Repositories.User;
using DbAccess.Specifications;
using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.SortSpecification;
using MyBlogAPI.DTO.User;

namespace MyBlogAPI.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
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

        private static void CheckUsernameValidity(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be null or empty.");
            if (!Regex.Match(username, @"^(?!.*[._()\[\]-]{2})[A-Za-z0-9._()\[\]-]{3,20}$").Success)
                throw new ArgumentException("Username must consist of between 3 to 20 allowed characters (A-Z, a-z, 0-9, .-_()[]) and cannot contain two consecutive symbols.");
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
            CheckEmailAddressValidity(user.EmailAddress);
            CheckUsernameValidity(user.Username);
            CheckPasswordValidity(user.Password);
            CheckUserDescription(user.UserDescription);
        }

        private async Task CheckUserValidity(AddUserDto user)
        {
            CheckUserValidity((IUserDto) user);
            if (await _repository.UsernameAlreadyExists(user.Username))
                throw new InvalidOperationException("Username already exists.");
            if (await _repository.EmailAddressAlreadyExists(user.EmailAddress))
                throw new InvalidOperationException("Email Address already exists.");
        }

        private async Task CheckUserValidity(UpdateUserDto user)
        {
            var userDb = _repository.GetAsync(user.Id);
            CheckUserValidity((IUserDto)user);
            if (await _repository.UsernameAlreadyExists(user.Username) &&
                (await userDb).UserName != user.Username)
                throw new InvalidOperationException("Username already exists.");
            if (await _repository.EmailAddressAlreadyExists(user.EmailAddress) &&
                (await userDb).EmailAddress != user.EmailAddress)
                throw new InvalidOperationException("Email Address already exists.");
        }

        public async Task<GetUserDto> AddUser(AddUserDto user)
        {
            await CheckUserValidity(user);
            var result = await _repository.AddAsync(_mapper.Map<User>(user));
           _unitOfWork.Save();
           return _mapper.Map<GetUserDto>(result);
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
            return userDb.UserName == user.Username &&
                   userDb.EmailAddress == user.EmailAddress &&
                   user.Password == userDb.Password &&
                   user.UserDescription == userDb.UserDescription;
        }
    }
}
