﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogCoreAPI.DTOs.Permission;
using BlogCoreAPI.DTOs.Role;
using DBAccess.Data.POCO;
using DBAccess.Data.POCO.Permission;
using DBAccess.Repositories.Role;
using DBAccess.Repositories.UnitOfWork;
using DBAccess.Specifications;
using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.SortSpecification;

namespace BlogCoreAPI.Services.RoleService
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
            return (await _repository.GetAllAsync()).Select(c =>
            {
                var roleDto = _mapper.Map<GetRoleDto>(c);
                roleDto.Users = c.UserRoles.Select(x => x.UserId);
                return roleDto;
            }).ToList();
        }

        public async Task<IEnumerable<GetRoleDto>> GetRoles(FilterSpecification<Role> filterSpecification = null, PagingSpecification pagingSpecification = null,
            SortSpecification<Role> sortSpecification = null)
        {
            return (await _repository.GetAsync(filterSpecification, pagingSpecification, sortSpecification)).Select(x => _mapper.Map<GetRoleDto>(x));
        }

        public async Task<int> CountRolesWhere(FilterSpecification<Role> filterSpecification = null)
        {
            return await _repository.CountWhereAsync(filterSpecification);
        }

        public async Task<GetRoleDto> GetRole(int id)
        {
            var role = await _repository.GetAsync(id);
            var roleDto = _mapper.Map<GetRoleDto>(role);
            roleDto.Users = role.UserRoles.Select(x => x.UserId);
            return roleDto;
        }

        public async Task<Role> GetRoleEntity(int id)
        {
            return await _repository.GetAsync(id);
        }

        public void CheckRoleValidity(IRoleDto role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));
            if (string.IsNullOrWhiteSpace(role.Name))
                throw new ArgumentException("Name cannot be null or empty.");
            if (role.Name.Length > 20)
                throw new ArgumentException("Name cannot exceed 20 characters.");
        }

        public async Task CheckRoleValidity(AddRoleDto role)
        {
            CheckRoleValidity((IRoleDto)role);
            if (await _repository.NameAlreadyExists(role.Name))
                throw new InvalidOperationException("Name already exists.");
        }

        public async Task CheckRoleValidity(UpdateRoleDto role)
        {
            CheckRoleValidity((IRoleDto)role);
            if (await _repository.NameAlreadyExists(role.Name) &&
                (await _repository.GetAsync(role.Id)).Name != role.Name)
                throw new InvalidOperationException("Name already exists.");
        }

        public async Task<GetRoleDto> AddRole(AddRoleDto role)
        {
            await CheckRoleValidity(role);
            var result = await _repository.AddAsync(_mapper.Map<Role>(role));
            _unitOfWork.Save();
            return _mapper.Map<GetRoleDto>(result);
        }

        public async Task UpdateRole(UpdateRoleDto role)
        {
            if (await RoleAlreadyExistsWithSameProperties(role))
                return;
            await CheckRoleValidity(role);
            var roleEntity = await _repository.GetAsync(role.Id);
            roleEntity.Name = role.Name;
            _unitOfWork.Save();
        }

        public async Task DeleteRole(int id)
        {
            await _repository.RemoveAsync(await _repository.GetAsync(id));
            _unitOfWork.Save();
        }

        private async Task<bool> RoleAlreadyExistsWithSameProperties(UpdateRoleDto role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));
            var roleDb = await _repository.GetAsync(role.Id);
            return role.Name == roleDb.Name;
        }

        public async Task AddPermissionAsync(int roleId, Permission permission)
        {
            await _repository.AddPermissionAsync(roleId, permission);
        }

        public async Task RemovePermissionAsync(int roleId, Permission permission)
        {
            await _repository.RemovePermissionAsync(roleId, permission);
        }

        public async Task<IEnumerable<PermissionDto>> GetPermissionsAsync(int roleId)
        {
            var permissions = await _repository.GetPermissionsAsync(roleId);
            return permissions.Select(x => _mapper.Map<PermissionDto>(x)).ToList();
        }
    }
}
