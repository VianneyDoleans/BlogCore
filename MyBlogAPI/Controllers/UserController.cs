using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyBlogAPI.DTO;
using MyBlogAPI.DTO.User;
using MyBlogAPI.Services.UserService;

namespace MyBlogAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _userService.GetAllUsers());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _userService.GetUser(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserDto user)
        {
            await _userService.AddUser(user);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(AddUserDto user)
        {
            if (await _userService.GetUser(user.Id) == null)
                return NotFound();
            await _userService.UpdateUser(user);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (await _userService.GetUser(id) == null)
                return NotFound();
            await _userService.DeleteUser(id);
            return Ok();
        }

    }
}
