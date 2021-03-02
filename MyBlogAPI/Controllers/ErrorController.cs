using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using MyBlogAPI.Errors;

namespace MyBlogAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        [Route("error")]
        public BlogErrorResponse Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context.Error;
            var code = HttpStatusCode.InternalServerError;

            switch (exception)
            {
                case IndexOutOfRangeException _:
                    code = HttpStatusCode.NotFound;
                    break;
                case InvalidOperationException _:
                    code = HttpStatusCode.Conflict;
                    break;
                case ArgumentNullException _:
                    code = HttpStatusCode.BadRequest;
                    break;
                case ArgumentException _:
                    code = HttpStatusCode.BadRequest;
                    break;
            }

            Response.StatusCode = (int) code;
            return new BlogErrorResponse(exception);
        }
    }
}
