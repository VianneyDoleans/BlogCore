using System.Linq;
using System.Net;
using AutoMapper;
using DbAccess.Data.POCO;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyBlogAPI.DTO.Category;
using MyBlogAPI.DTO.Category.Converters;
using MyBlogAPI.DTO.Comment;
using MyBlogAPI.DTO.Comment.Converters;
using MyBlogAPI.DTO.Like;
using MyBlogAPI.DTO.Like.Converters;
using MyBlogAPI.DTO.Post;
using MyBlogAPI.DTO.Post.Converters;
using MyBlogAPI.DTO.Role;
using MyBlogAPI.DTO.Role.Converters;
using MyBlogAPI.DTO.Tag;
using MyBlogAPI.DTO.Tag.Converters;
using MyBlogAPI.DTO.User;
using MyBlogAPI.DTO.User.Converters;

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
