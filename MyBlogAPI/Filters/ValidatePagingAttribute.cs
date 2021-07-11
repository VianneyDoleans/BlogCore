using AutoMapper;

namespace MyBlogAPI.Filters
{
    public class ValidatePagingAttribute : Profile
    {
        //public override void OnActionExecuting(HttpActionContext actionContext*/)
        //{
           /* string order = (string)actionContext.ActionArguments["order"];
            string sortBy = (string)actionContext.ActionArguments["sortby"];

            var states = new ModelStateDictionary();
            if (!order.Equals("ASC") && !order.Equals("DESC"))
            {
                states.AddModelError("order", "Order has to be DESC or ASC");
            }

            if (!new[] { "username", "name" }.Contains(sortBy.ToLower()))
            {
                states.AddModelError("sortby", "Not A Valid Sorting Column");
            }

            if (states.Any())
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, states);
            }*/
        //}
    }
}
