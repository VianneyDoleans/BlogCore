using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DbAccess.Data.POCO;
using DbAccess.Repositories;
using DbAccess.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("GetAll")]
        public async Task<IEnumerable<GetUserDto>> GetAllUsers()
        {
            return _repository.GetAll().Select(c => _mapper.Map<GetUserDto>(c)).ToList();
        }

        [HttpGet("{id}")]
        public async Task<GetUserDto> GetUser(int id)
        {
            return _mapper.Map<GetUserDto>(_repository.Get(id));
        }

        [HttpPost]
        public async Task AddUser(AddUserDto user)
        {
           var pocoUser = _mapper.Map<User>(user);
           _repository.Add(pocoUser);
           _unitOfWork.Save();
        }

        [HttpPut]
        public async Task UpdateUser(AddUserDto user)
        {
            var userToModify = _repository.Get(user.Id);
            userToModify.Username = user.Username;
            userToModify.EmailAddress = user.EmailAddress;
            userToModify.Password = user.Password;
            userToModify.UserDescription = user.UserDescription;
            _unitOfWork.Save();
        }

        [HttpDelete]
        public async Task DeleteUser(int id)
        {
            _repository.Remove(_repository.Get(id));
            _unitOfWork.Save();
        }
    }
}
