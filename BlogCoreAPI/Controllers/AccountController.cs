using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlogCoreAPI.Authorization.Permissions;
using BlogCoreAPI.Models.DTOs.Account;
using BlogCoreAPI.Models.DTOs.Immutable;
using BlogCoreAPI.Models.Exceptions;
using BlogCoreAPI.Models.Mails;
using BlogCoreAPI.Responses;
using BlogCoreAPI.Services.JwtService;
using BlogCoreAPI.Services.MailService;
using BlogCoreAPI.Services.UserService;
using DBAccess.Data;
using DBAccess.Data.Permission;
using DBAccess.Specifications.FilterSpecifications.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogCoreAPI.Controllers
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
        private readonly IAuthorizationService _authorizationService;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="jwtService"></param>
        /// <param name="authorizationService"></param>
        /// <param name="emailService"></param>
        public AccountController(IUserService userService, IJwtService jwtService,
            IAuthorizationService authorizationService, IEmailService emailService)
        {
            _userService = userService;
            _jwtService = jwtService;
            _authorizationService = authorizationService;
            _emailService = emailService;
        }

        /// <summary>
        /// Confirm email account by giving the token received by email
        /// </summary>
        [HttpGet("Email/Confirmation")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetAccountDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmAccountEmail(string emailValidationToken, int userId)
        {
            if (await _userService.EmailIsConfirmed(userId))
                return BadRequest(new BlogErrorResponse(nameof(InvalidRequestException), "Email already confirmed."));
            var emailConfirmation = await _userService.ConfirmEmail(emailValidationToken, userId);
            if (!emailConfirmation)
                return BadRequest(new BlogErrorResponse(nameof(InvalidRequestException), "Bad email validation token or user Id."));
            return Ok();
        }
        
        /// <summary>
        /// Reset password account by giving the token received by email
        /// </summary>
        [HttpGet("Password/Reset")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetAccountDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetAccountPassword(string passwordResetToken, int userId, string newPassword)
        {
            await _userService.ResetPassword(passwordResetToken, userId, newPassword);
            return Ok();
        }
        
        /// <summary>
        /// Send an email to reset password account
        /// </summary>
        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetAccountDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgotPassword(ResetPasswordDto resetPassword, CancellationToken token)
        {
            var user = (await _userService.GetUsers(new EmailEqualsSpecification<User>(resetPassword.Email))).FirstOrDefault();
            if (user == null)
                return Ok();
            var accountGet = await _userService.GetAccount(user.Id);
            var passwordResetToken = await _userService.GeneratePasswordResetToken(user.Id);
            await _emailService.SendEmailAsync(new Message(new List<EmailIdentity>() {new(user.UserName, accountGet.Email)}, "Reset your password", 
                $"Hello {accountGet.UserName},<br/><br/>Here is a password reset token needed to reset your password on the API: {passwordResetToken}." +
                $"<br/><br/>If you have not requested to reset your password, you can ignore this email."), token);
           
            return Ok(accountGet);
        }

        /// <summary>
        /// Create an account (a user).
        /// </summary>
        /// <remarks>
        /// Create a user.
        /// </remarks>
        /// <param name="account"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost("SignUp")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetAccountDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> SignUp(AddAccountDto account, CancellationToken token)
        {
           var accountGet = await _userService.AddAccount(account);
           
           var emailValidationToken = await _userService.GenerateConfirmEmailToken(accountGet.Id);
           var confirmationLink = Url.Action("ConfirmAccountEmail", "Account", new { emailValidationToken, userId = accountGet.Id }, Request.Scheme);
           await _emailService.SendEmailAsync(new Message(new List<EmailIdentity>() {new(accountGet.UserName, accountGet.Email)}, "Confirm your email", 
               $"Hello {accountGet.UserName},<br/><br/>To confirm your registration, please verify your email by clicking on this link:<br/>{confirmationLink}."), token);
           
           return Ok(accountGet);
        }

        /// <summary>
        /// Sign In as a user.
        /// </summary>
        /// <remarks>
        /// Sign In as a user.
        /// </remarks>
        /// <param name="accountLogin"></param>
        /// <returns></returns>
        [HttpPost("SignIn")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(JsonWebToken), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignIn(AccountLoginDto accountLogin)
        {
            var user = await _userService.GetAccount(accountLogin.UserName);
            
            if (!await _userService.SignIn(accountLogin))
                return BadRequest(new BlogErrorResponse(nameof(InvalidRequestException),"Bad username or password."));
            if (!await _userService.EmailIsConfirmed(user.Id))
                return BadRequest(new BlogErrorResponse(nameof(InvalidRequestException),"Email must be confirmed before you can sign in."));
            return Ok(await _jwtService.GenerateJwt(user.Id));
        }

        /// <summary>
        /// Update a user.
        /// </summary>
        /// <remarks>
        /// Update a user.
        /// </remarks>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateAccount(UpdateAccountDto account)
        {
            if (await _userService.GetAccount(account.Id) == null)
                return NotFound();

            var userEntity = await _userService.GetUserEntity(account.Id);
            var authorized = await _authorizationService.AuthorizeAsync(User, userEntity, new PermissionRequirement(PermissionAction.CanUpdate, PermissionTarget.Account));
            if (!authorized.Succeeded)
                return Forbid();

            await _userService.UpdateAccount(account);
            return Ok();
        }

        /// <summary>
        /// Get a user by giving its Id.
        /// </summary>
        /// <remarks>
        /// Get a user by giving its Id.
        /// </remarks>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId:int}")]
        [ProducesResponseType(typeof(GetAccountDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAccount(int userId)
        {
            var userEntity = await _userService.GetUserEntity(userId);
            var authorized = await _authorizationService.AuthorizeAsync(User, userEntity, new PermissionRequirement(PermissionAction.CanRead, PermissionTarget.Account));
            if (!authorized.Succeeded)
                return Forbid();
            return Ok(await _userService.GetAccount(userId));
        }

        /// <summary>
        /// Delete a user by giving its id.
        /// </summary>
        /// <remarks>
        /// Delete a user by giving its id.
        /// </remarks>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("{userId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            if (await _userService.GetUser(userId) == null)
                return NotFound();

            var userEntity = await _userService.GetUserEntity(userId);
            var authorized = await _authorizationService.AuthorizeAsync(User, userEntity, new PermissionRequirement(PermissionAction.CanDelete, PermissionTarget.Account));
            if (!authorized.Succeeded)
                return Forbid();

            await _userService.DeleteAccount(userId);
            return Ok();
        }
    }
}
