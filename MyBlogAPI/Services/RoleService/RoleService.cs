using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DbAccess.Data.POCO;
using DbAccess.Repositories.Role;
using DbAccess.Repositories.UnitOfWork;
using MyBlogAPI.DTO.Role;

namespace MyBlogAPI.Services.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IRoleRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GetRoleDto>> GetAllRoles()
        {
            return _repository.GetAll().Select(c =>
            {
                var roleDto = _mapper.Map<GetRoleDto>(c);
                roleDto.Users = c.UserRoles.Select(x => x.UserId);
                return roleDto;
            }).ToList();
        }

        public async Task<GetRoleDto> GetRole(int id)
        {
            try
            {
                var role = _repository.Get(id);
                var roleDto = _mapper.Map<GetRoleDto>(role);
                roleDto.Users = role.UserRoles.Select(x => x.UserId);
                return roleDto;
            }
            catch (InvalidOperationException)
            {
                throw new IndexOutOfRangeException("Role doesn't exist.");
            }
        }

        public async Task CheckRoleValidity(AddRoleDto role)
        {
            if (role == null)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(role.Name))
                throw new ArgumentException("Name cannot be null or empty.");
            if (role.Name.Length > 20)
                throw new ArgumentException("Name cannot exceed 20 characters.");
            if (await _repository.NameAlreadyExists(role.Name))
                throw new InvalidOperationException("Name already exists.");
        }

        public async Task CheckRoleValidity(UpdateRoleDto role)
        {
            if (role == null)
                throw new ArgumentNullException();
            if (_repository.GetAsync(role.Id) == null)
                throw new ArgumentException("Role doesn't exist.");
            if (string.IsNullOrWhiteSpace(role.Name))
                throw new ArgumentException("Name cannot be null or empty.");
            if (role.Name.Length > 20)
                throw new ArgumentException("Name cannot exceed 20 characters.");
            if (await _repository.NameAlreadyExists(role.Name))
                throw new InvalidOperationException("Name already exists.");
        }

        public async Task<GetRoleDto> AddRole(AddRoleDto role)
        {
            await CheckRoleValidity(role);
            var result = _repository.Add(_mapper.Map<Role>(role));
            _unitOfWork.Save();
            return _mapper.Map<GetRoleDto>(result);
        }

        public async Task UpdateRole(UpdateRoleDto role)
        {
            await CheckRoleValidity(role);
            var roleEntity = _repository.Get(role.Id);
            roleEntity.Name = role.Name;
            _unitOfWork.Save();
        }

        public async Task DeleteRole(int id)
        {
            _repository.Remove(_repository.Get(id));
            _unitOfWork.Save();
        }
    }
}
