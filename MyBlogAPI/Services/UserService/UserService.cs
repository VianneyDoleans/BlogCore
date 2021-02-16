using System.Collections.Generic;
using System.Linq;
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
            var user = _repository.Get(id);
            var userDto = _mapper.Map<GetUserDto>(user);
            userDto.Roles = user.UserRoles.Select(x => x.RoleId);
            return userDto;
        }

        public async Task AddUser(AddUserDto user)
        {
            _repository.Add(_mapper.Map<User>(user));
           _unitOfWork.Save();
        }

        public async Task UpdateUser(AddUserDto user)
        {
            var userToModify = _repository.Get(user.Id);
            userToModify.Username = user.Username;
            userToModify.EmailAddress = user.EmailAddress;
            userToModify.Password = user.Password;
            userToModify.UserDescription = user.UserDescription;
            _unitOfWork.Save();
        }

        public async Task DeleteUser(int id)
        {
            _repository.Remove(_repository.Get(id));
            _unitOfWork.Save();
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
