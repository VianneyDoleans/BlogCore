using System;
using System.Net;
using BlogCoreAPI.Models.Exceptions;
using BlogCoreAPI.Responses;
using DBAccess.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogCoreAPI.Controllers
{
    /// <summary>
    /// Errors controllers used as middleware to catch exceptions and manage behaviors.
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        /// <summary>
        /// Define HttpStatusCode and error type depending of exception raised.
        /// </summary>
        /// <returns></returns>
        [Route("error")]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        public BlogErrorResponse Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context.Error;
            var code = HttpStatusCode.InternalServerError;

            switch (exception)
            {
                case ResourceNotFoundException:
                    code = HttpStatusCode.NotFound;
                    break;
                case InvalidRequestException:
                case PermissionManagementException:
                case RoleManagementException:
                case UserManagementException:
                    code = HttpStatusCode.Conflict;
                    break;
                case ArgumentNullException:
                case ArgumentException:
                case ValidationException:
                    code = HttpStatusCode.BadRequest;
                    break;
            }

            Response.StatusCode = (int) code;
            return new BlogErrorResponse(exception.GetType().Name, exception.Message);
        }
    }
}
