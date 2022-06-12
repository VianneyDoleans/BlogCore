using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogCoreAPI.DTOs.User;
using DBAccess.Data.POCO;
using DBAccess.Repositories.Role;
using DBAccess.Repositories.UnitOfWork;
using DBAccess.Repositories.User;
using DBAccess.Specifications;
using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.FilterSpecifications.Filters;
using DBAccess.Specifications.SortSpecification;
using FluentValidation;

namespace BlogCoreAPI.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<IUserDto> _dtoValidator;

        public UserService(IUserRepository repository, IRoleRepository roleRepository, IMapper mapper, IUnitOfWork unitOfWork,
            IValidator<IUserDto> dtoValidator)
        {
            _repository = repository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _dtoValidator = dtoValidator;
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

        public async Task<IEnumerable<GetUserDto>> GetUsers(FilterSpecification<User> filterSpecification = null, PagingSpecification pagingSpecification = null,
            SortSpecification<User> sortSpecification = null)
        {
            return (await _repository.GetAsync(filterSpecification, pagingSpecification, sortSpecification)).Select(x =>
            {
                var userDto = _mapper.Map<GetUserDto>(x);
                userDto.Roles = x.UserRoles.Select(y => y.RoleId);
                return userDto;
            });
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

        public async Task<GetUserDto> GetUser(string userName)
        {
            var user = await FindUser(userName);
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

            if (user == null || user.Count != 1)
            {
                throw new IndexOutOfRangeException("User doesn't exists.");
            }
            return user.First();
        }

        private async Task CheckUserValidity(AddUserDto user)
        {
            if (await _repository.UserNameAlreadyExists(user.UserName))
                throw new InvalidOperationException("UserName already exists.");
            if (await _repository.EmailAlreadyExists(user.Email))
                throw new InvalidOperationException("Email Address already exists.");
        }

        private async Task CheckUserValidity(UpdateUserDto user)
        {
            var userDb = _repository.GetAsync(user.Id);
            if (await _repository.UserNameAlreadyExists(user.UserName) &&
                (await userDb).UserName != user.UserName)
                throw new InvalidOperationException("UserName already exists.");
            if (await _repository.EmailAlreadyExists(user.Email) &&
                (await userDb).Email != user.Email)
                throw new InvalidOperationException("Email Address already exists.");
        }

        public async Task<GetUserDto> AddUser(AddUserDto user)
        {
            await _dtoValidator.ValidateAndThrowAsync(user);
            await CheckUserValidity(user);
            var userToAdd = _mapper.Map<User>(user);
            var result = await _repository.AddAsync(userToAdd);
           _unitOfWork.Save();
           var userDto = _mapper.Map<GetUserDto>(result);
            userDto.Roles = result.UserRoles.Select(x => x.RoleId);
            return userDto;
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
            await _dtoValidator.ValidateAndThrowAsync(user);
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
