using System;
using System.Threading.Tasks;
using DbAccess.Specifications;
using Microsoft.AspNetCore.Mvc;
using MyBlogAPI.DTO.User;
using MyBlogAPI.Filters;
using MyBlogAPI.Filters.Post;
using MyBlogAPI.Filters.User;
using MyBlogAPI.Services.CommentService;
using MyBlogAPI.Services.LikeService;
using MyBlogAPI.Services.PostService;
using MyBlogAPI.Services.UserService;

namespace MyBlogAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILikeService _likeService;
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;

        public UsersController(IUserService userService, ILikeService likeService, IPostService postService, 
            ICommentService commentService)
        {
            _userService = userService;
            _likeService = likeService;
            _postService = postService;
            _commentService = commentService;
        }

        [HttpGet("All")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _userService.GetAllUsers());
        }

        [HttpGet()]
        public async Task<IActionResult> GetUsers(string sortingDirection = "ASC", string orderBy = null, int pageNumber = 1,
            int pageSize = 10, string name = null, DateTime? registerBefore = null, DateTime? lastLoginBefore = null)
        {
            var validFilter = new PaginationFilter(pageNumber, pageSize);

            return Ok(await _userService.GetUsers(new UserQueryFilter(name, lastLoginBefore, registerBefore).GetFilterSpecification(),
                new PagingSpecification((validFilter.PageNumber - 1) * validFilter.PageSize, validFilter.PageSize),
                new SortUserFilter(sortingDirection, orderBy).GetSorting()));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _userService.GetUser(id));
        }

        [HttpGet("{id:int}/Posts/")]
        public async Task<IActionResult> GetPostsFromUser(int id)
        {
            return Ok(await _postService.GetPostsFromUser(id));
        }

        [HttpGet("{id:int}/Comments/")]
        public async Task<IActionResult> GetCommentsFromUser(int id)
        {
            return Ok(await _commentService.GetCommentsFromUser(id));
        }

        [HttpGet("{id:int}/Likes/")]
        public async Task<IActionResult> GetLikesFromUser(int id)
        {
            return Ok(await _likeService.GetLikesFromUser(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserDto user)
        {
            return Ok(await _userService.AddUser(user));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserDto user)
        {
            if (await _userService.GetUser(user.Id) == null)
                return NotFound();
            await _userService.UpdateUser(user);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (await _userService.GetUser(id) == null)
                return NotFound();
            await _userService.DeleteUser(id);
            return Ok();
        }
    }
}
