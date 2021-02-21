using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DbAccess.Data.POCO;
using DbAccess.Repositories.Role;
using DbAccess.Repositories.UnitOfWork;
using MyBlogAPI.DTO;
using MyBlogAPI.DTO.Role;
using MyBlogAPI.DTO.User;

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
            //return _repository.GetAll().Select(x => _mapper.Map<GetRoleDto>(x)).ToList();
            return _repository.GetAll().Select(c =>
            {
                var roleDto = _mapper.Map<GetRoleDto>(c);
                roleDto.Users = c.UserRoles.Select(x => x.UserId);
                return roleDto;
            }).ToList();
        }

        public async Task<GetRoleDto> GetRole(int id)
        {
            var role = _repository.Get(id);
            var roleDto = _mapper.Map<GetRoleDto>(role);
            roleDto.Users = role.UserRoles.Select(x => x.UserId);
            return roleDto;
            //return _mapper.Map<GetRoleDto>(_repository.Get(id));
        }

        public async Task<GetRoleDto> AddRole(AddRoleDto role)
        {
            var result = _repository.Add(_mapper.Map<Role>(role));
            _unitOfWork.Save();
            return _mapper.Map<GetRoleDto>(result);
        }

        public async Task UpdateRole(UpdateRoleDto role)
        {
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
