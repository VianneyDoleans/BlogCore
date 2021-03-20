﻿using System;
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

        private async Task<Role> GetRoleFromRepository(int id)
        {
            try
            {
                var roleDb = await _repository.GetAsync(id);
                if (roleDb == null)
                    throw new IndexOutOfRangeException("Role doesn't exist.");
                return roleDb;
            }
            catch
            {
                throw new IndexOutOfRangeException("Role doesn't exist.");
            }
        }

        public async Task<GetRoleDto> GetRole(int id)
        {
            var role = await GetRoleFromRepository(id);
            var roleDto = _mapper.Map<GetRoleDto>(role);
            roleDto.Users = role.UserRoles.Select(x => x.UserId);
            return roleDto;
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
            var roleDb = await GetRoleFromRepository(role.Id);
            if (role.Name == roleDb.Name)
                return;
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
            var result = await _repository.AddAsync(_mapper.Map<Role>(role));
            _unitOfWork.Save();
            return _mapper.Map<GetRoleDto>(result);
        }

        public async Task UpdateRole(UpdateRoleDto role)
        {
            await CheckRoleValidity(role);
            var roleEntity = await GetRoleFromRepository(role.Id);
            roleEntity.Name = role.Name;
            _unitOfWork.Save();
        }

        public async Task DeleteRole(int id)
        {
            await _repository.RemoveAsync(await GetRoleFromRepository(id));
            _unitOfWork.Save();
        }
    }
}
