using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DbAccess.Specifications;
using MyBlogAPI.DTO.Post;
using MyBlogAPI.Filters;
using MyBlogAPI.Filters.Post;
using MyBlogAPI.Services.CommentService;
using MyBlogAPI.Services.LikeService;
using MyBlogAPI.Services.PostService;

namespace MyBlogAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;
        private readonly ILikeService _likeService;

        public PostsController(IPostService postService, ICommentService commentService, ILikeService likeService)
        {
            _postService = postService;
            _commentService = commentService;
            _likeService = likeService;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _postService.GetAllPosts());
        }

        [HttpGet()]
        public async Task<IActionResult> GetPosts(string sortingDirection = "ASC", string orderBy = null, int offset = 1,
            int limit = 10, string name = null, string content = null)
        {
            var validFilter = new PaginationFilter(offset, limit);

            return Ok(await _postService.GetPosts(new PostQueryFilter(content, name).GetFilterSpecification(),
                new PagingSpecification((validFilter.Offset - 1) * validFilter.Limit, validFilter.Limit),
                new SortPostFilter(sortingDirection, orderBy).GetSorting()));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _postService.GetPost(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(AddPostDto post)
        {
            return Ok(await _postService.AddPost(post));
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePost(UpdatePostDto post)
        {
            if (await _postService.GetPost(post.Id) == null)
                return NotFound();
            await _postService.UpdatePost(post);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            if (await _postService.GetPost(id) == null)
                return NotFound();
            await _postService.DeletePost(id);
            return Ok();
        }

        [HttpGet("{id:int}/Comments/")]
        public async Task<IActionResult> GetCommentsFromPost(int id)
        {
            return Ok(await _commentService.GetCommentsFromPost(id));
        }

        [HttpGet("{id:int}/Likes/")]
        public async Task<IActionResult> GetLikesFromPost(int id)
        {
            return Ok(await _likeService.GetLikesFromPost(id));
        }
    }
}
