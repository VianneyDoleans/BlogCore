﻿using System.Threading.Tasks;
using DbAccess.Data.POCO;
using DbAccess.Data.POCO.Permission;
using DbAccess.Specifications.FilterSpecifications.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlogAPI.Attributes;
using MyBlogAPI.DTO.User;
using MyBlogAPI.Responses;
using MyBlogAPI.Services.JwtService;
using MyBlogAPI.Services.UserService;

namespace MyBlogAPI.Controllers
{
    /// <summary>
    /// Controller used to enables account action such as login / log out.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userService"></param>
        public AccountController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Create an account (a user).
        /// </summary>
        /// <remarks>
        /// Create a user.
        /// </remarks>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("SignUp")]
        [AllowAnonymous]
        [PermissionRequired(PermissionAction.CanCreate, PermissionTarget.User)]
        [ProducesResponseType(typeof(GetUserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> SignUp(AddUserDto user)
        {
            return Ok(await _userService.AddUser(user));
        }

        /// <summary>
        /// Sign In as a user.
        /// </summary>
        /// <remarks>
        /// Sign In as a user.
        /// </remarks>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        [HttpPost("SignIn")]
        [AllowAnonymous]
        [PermissionRequired(PermissionAction.CanCreate, PermissionTarget.User)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignIn(UserLoginDto userLogin)
        {
            if (await _userService.SignIn(userLogin))
            {
                return Ok(await _jwtService.GenerateJwt((await _userService.GetUser(userLogin.UserName)).Id));
            }
            return BadRequest("Bad username or password.");
        }
    }
}
