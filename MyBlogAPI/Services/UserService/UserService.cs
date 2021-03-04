using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using DbAccess.Data.POCO;
using DbAccess.Repositories.UnitOfWork;
using DbAccess.Repositories.User;
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

        public async Task<GetUserDto> GetUser(int id)
        {
            var user = await GetUserFromRepository(id);
                var userDto = _mapper.Map<GetUserDto>(user);
                userDto.Roles = user.UserRoles.Select(x => x.RoleId);
                return userDto;
        }

        private async Task CheckUsernameValidity(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be null or empty.");
            if (!Regex.Match(username, @"^(?!.*[._()\[\]-]{2})[A-Za-z0-9._()\[\]-]{3,20}$").Success)
                throw new ArgumentException("Username must consist of between 3 to 20 allowed characters (A-Z, a-z, 0-9, .-_()[]) and cannot contain two consecutive symbols.");
            if (await _repository.UsernameAlreadyExists(username))
                throw new InvalidOperationException("Username already exists.");
        }

        private async Task CheckEmailAddressValidity(string emailAddress)
        {
            if (string.IsNullOrWhiteSpace(emailAddress))
                throw new ArgumentException("Email cannot be null or empty.");
            if (emailAddress.Length > 320)
                throw new ArgumentException("Email address cannot exceed 320 characters.");
            if (!Regex.Match(emailAddress,
                    @"\A[a-zA-Z0-9.!\#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*\z")
                .Success)
                throw new ArgumentException("Email address is invalid.");
            if (await _repository.EmailAddressAlreadyExists(emailAddress))
                throw new InvalidOperationException("Email Address already exists.");
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

        private async Task<User> GetUserFromRepository(int id)
        {
            // TODO problem here if User.Id is null
            try
            {
                var userDb = await _repository.GetAsync(id);
                if (userDb == null)
                    throw new IndexOutOfRangeException("User doesn't exist.");
                return userDb;
            }
            catch
            {
                throw new IndexOutOfRangeException("User doesn't exist.");
            }
        }

        private async Task CheckUserValidity(AddUserDto user)
        {
            var emailTask = CheckEmailAddressValidity(user.EmailAddress);
            var usernameTask = CheckUsernameValidity(user.Username);
            CheckPasswordValidity(user.Password);
            CheckUserDescription(user.UserDescription);

            await emailTask;
            await usernameTask;
        }

        private async Task CheckUserValidity(UpdateUserDto user)
        {
            Task emailTask = null;
            Task usernameTask = null;

            var userDb = await GetUserFromRepository(user.Id);
            if (userDb.EmailAddress != user.EmailAddress) 
                emailTask = CheckEmailAddressValidity(user.EmailAddress);
            if (userDb.Username != user.Username) 
                usernameTask = CheckUsernameValidity(user.Username);
            CheckPasswordValidity(user.Password);
            CheckUserDescription(user.UserDescription);

            if (emailTask != null) 
                await emailTask;
            if (usernameTask != null) 
                await usernameTask;
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
            await CheckUserValidity(user);
            var userEntity = await GetUserFromRepository(user.Id);
            _mapper.Map(user, userEntity);
            _unitOfWork.Save();
        }

        public async Task DeleteUser(int id)
        {
            await _repository.RemoveAsync(await GetUserFromRepository(id));
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
    }
}
