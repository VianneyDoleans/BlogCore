using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DbAccess.Repositories.Like;
using MyBlogAPI.DTO.Post;
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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _postService.GetAllPosts());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _postService.GetPost(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(AddPostDto post)
        {
            await _postService.AddPost(post);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePost(AddPostDto post)
        {
            if (await _postService.GetPost(post.Id) == null)
                return NotFound();
            await _postService.UpdatePost(post);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            if (await _postService.GetPost(id) == null)
                return NotFound();
            await _postService.DeletePost(id);
            return Ok();
        }

        [HttpGet("{id}/Comments/")]
        public async Task<IActionResult> GetCommentsFromPost(int id)
        {
            return Ok(await _commentService.GetCommentsFromPost(id));
        }

        [HttpGet("{id}/Likes/")]
        public async Task<IActionResult> GetLikesFromPost(int id)
        {
            return Ok(await _likeService.GetLikesFromPost(id));
        }
    }
}
