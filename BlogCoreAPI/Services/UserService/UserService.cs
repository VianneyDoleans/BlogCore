using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogCoreAPI.Models.Constants;
using BlogCoreAPI.Models.DTOs.Account;
using BlogCoreAPI.Models.DTOs.User;
using BlogCoreAPI.Models.Exceptions;
using DBAccess.Data;
using DBAccess.Exceptions;
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
        private readonly IValidator<IAccountDto> _accountDtoValidator;

        public UserService(IUserRepository repository, IRoleRepository roleRepository, IMapper mapper, IUnitOfWork unitOfWork,
            IValidator<IAccountDto> accountDtoValidator)
        {
            _repository = repository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _accountDtoValidator = accountDtoValidator;
        }

        public async Task<IEnumerable<GetAccountDto>> GetAllAccounts()
        {
            var users = await _repository.GetAllAsync();
            return users.Select(c =>
            {
               var usersDto = _mapper.Map<GetAccountDto>(c);
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

        public async Task<GetAccountDto> GetAccount(int id)
        {
            var user = await _repository.GetAsync(id);
            var userDto = _mapper.Map<GetAccountDto>(user);
            userDto.Roles = user.UserRoles.Select(x => x.RoleId);
            return userDto;
        }

        public async Task<GetUserDto> GetUser(int id)
        {
            var user = await _repository.GetAsync(id);
            var userDto = _mapper.Map<GetUserDto>(user);
            userDto.Roles = user.UserRoles.Select(x => x.RoleId);
            return userDto;
        }

        public async Task<GetAccountDto> GetAccount(string userName)
        {
            var user = await FindUser(userName);
            var userDto = _mapper.Map<GetAccountDto>(user);
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
                throw new ResourceNotFoundException("User doesn't exists.");
            }
            return user.First();
        }

        private async Task CheckUserValidity(AddAccountDto account)
        {
            if (await _repository.UserNameAlreadyExists(account.UserName))
                throw new InvalidRequestException("UserName already exists.");
            if (await _repository.EmailAlreadyExists(account.Email))
                throw new InvalidRequestException("Email Address already exists.");
            if (string.IsNullOrEmpty(account.Password))
                throw new ValidationException(UserMessage.CannotBeNullOrEmpty(nameof(account.Password)));
        }

        private async Task CheckUserValidity(UpdateAccountDto account)
        {
            var userDb = _repository.GetAsync(account.Id);
            if (await _repository.UserNameAlreadyExists(account.UserName) &&
                (await userDb).UserName != account.UserName)
                throw new InvalidRequestException("UserName already exists.");
            if (await _repository.EmailAlreadyExists(account.Email) &&
                (await userDb).Email != account.Email)
                throw new InvalidRequestException("Email Address already exists.");
        }

        public async Task<GetAccountDto> AddAccount(AddAccountDto account)
        {
            await _accountDtoValidator.ValidateAndThrowAsync(account);
            await CheckUserValidity(account);
            var userToAdd = _mapper.Map<User>(account);
            var result = await _repository.AddAsync(userToAdd);
           _unitOfWork.Save();
           var userDto = _mapper.Map<GetAccountDto>(result);
            userDto.Roles = result.UserRoles.Select(x => x.RoleId);
            return userDto;
        }

        public async Task AddUserRole(UserRoleDto userRole)
        {
            var user = (await _repository.GetAsync(userRole.UserId));
            if (user == null)
            {
                throw new ResourceNotFoundException("User doesn't exists.");
            }

            var role = (await _roleRepository.GetAsync(userRole.RoleId));
            if (role == null)
            {
                throw new ResourceNotFoundException("Role doesn't exists.");
            }

            if (user.UserRoles.Any(x => x.UserId == userRole.UserId && x.RoleId == userRole.RoleId))
                throw new InvalidRequestException("User already have the role.");
            await _repository.AddRoleToUser(user, role);
            _unitOfWork.Save();
        }

        public async Task RemoveUserRole(UserRoleDto userRole)
        {
            var user = (await _repository.GetAsync(userRole.UserId));
            if (user == null)
            {
                throw new ResourceNotFoundException("User doesn't exists.");
            }

            var role = (await _roleRepository.GetAsync(userRole.RoleId));
            if (role == null)
            {
                throw new ResourceNotFoundException("Role doesn't exists.");
            }

            if (!user.UserRoles.Any(x => x.UserId == userRole.UserId && x.RoleId == userRole.RoleId))
                throw new InvalidRequestException("User doesn't have the role.");
            await _repository.RemoveRoleToUser(user, role);
            _unitOfWork.Save();
        }

        public async Task<bool> SignIn(AccountLoginDto accountLogin)
        {
            var user = await FindUser(accountLogin.UserName);

            var isValidPassword = await _repository.CheckPasswordAsync(user, accountLogin.Password);
            return isValidPassword;
        }

        public async Task UpdateAccount(UpdateAccountDto account)
        {
            await _accountDtoValidator.ValidateAndThrowAsync(account);
            if (await UserAlreadyExistsWithSameProperties(account))
                return;
            await CheckUserValidity(account);
            var userEntity = await _repository.GetAsync(account.Id);
            _mapper.Map(account, userEntity);
            _unitOfWork.Save();
        }

        public async Task DeleteAccount(int id)
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

        private async Task<bool> UserAlreadyExistsWithSameProperties(UpdateAccountDto account)
        {
            var userDb = await _repository.GetAsync(account.Id);
            return userDb.UserName == account.UserName &&
                   userDb.Email == account.Email &&
                   account.UserDescription == userDb.UserDescription &&
                   account.ProfilePictureUrl == userDb.ProfilePictureUrl;
        }
    }
}
