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
            try
            {
                var user = await _repository.GetAsync(id);
                var userDto = _mapper.Map<GetUserDto>(user);
                userDto.Roles = user.UserRoles.Select(x => x.RoleId);
                return userDto;
            }
            catch (InvalidOperationException)
            {
                throw new IndexOutOfRangeException("User doesn't exist.");
            }
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

        private async Task CheckUserValidity(AddUserDto user)
        {
            if (user == null)
                throw new ArgumentNullException();
            var emailTask = CheckEmailAddressValidity(user.EmailAddress);
            var usernameTask = CheckUsernameValidity(user.Username);
            if (string.IsNullOrWhiteSpace(user.Password))
                throw new ArgumentException("Password cannot be null or empty.");
            if (user.UserDescription != null && user.UserDescription.Length > 1000) 
                throw new ArgumentException("User description cannot exceed 1000 characters.");
            await emailTask;
            await usernameTask;
        }

        private async Task CheckUserValidity(UpdateUserDto user)
        {
            if (user == null)
                throw new ArgumentNullException();
            if (_repository.GetAsync(user.Id) == null)
                throw new ArgumentException("User doesn't exist.");
            var emailTask = CheckEmailAddressValidity(user.EmailAddress);
            var usernameTask = CheckUsernameValidity(user.Username);
            if (string.IsNullOrWhiteSpace(user.Password))
                throw new ArgumentException("Password cannot be null or empty.");
            if (user.UserDescription != null && user.UserDescription.Length > 1000)
                throw new ArgumentException("User description cannot exceed 1000 characters.");
            await emailTask;
            await usernameTask;
        }

        public async Task<GetUserDto> AddUser(AddUserDto user)
        {
            await CheckUserValidity(user);
            var result = _repository.Add(_mapper.Map<User>(user));
           _unitOfWork.Save();
           return _mapper.Map<GetUserDto>(result);

        }

        public async Task UpdateUser(UpdateUserDto user)
        {
            try
            {
                await CheckUserValidity(user);
                var userToModify = _repository.Get(user.Id);
                userToModify.Username = user.Username;
                userToModify.EmailAddress = user.EmailAddress;
                userToModify.Password = user.Password;
                userToModify.UserDescription = user.UserDescription;
                _unitOfWork.Save();
            }
            catch (InvalidOperationException)
            {
                throw new IndexOutOfRangeException("User doesn't exist.");
            }
        }

        public async Task DeleteUser(int id)
        {
            try
            {
                _repository.Remove(_repository.Get(id));
                _unitOfWork.Save();
            }
            catch (InvalidOperationException)
            {
                throw new IndexOutOfRangeException("User doesn't exist.");
            }
        }

        public async Task<IEnumerable<GetUserDto>> GetUsersFromRole(int id)
        {
            var users = await _repository.GetUsersFromRole(id);
            var userDtos = users.Select(x =>
            {
                var userDto = _mapper.Map<GetUserDto>(x);
                userDto.Roles = x.UserRoles.Select(y => y.RoleId);
                return userDto;
            }).ToList();
            return userDtos;
        }
    }
}
