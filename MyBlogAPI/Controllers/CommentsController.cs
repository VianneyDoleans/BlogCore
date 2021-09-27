using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DbAccess.Specifications;
using MyBlogAPI.DTO.Comment;
using MyBlogAPI.Filters;
using MyBlogAPI.Filters.Comment;
using MyBlogAPI.Services.CommentService;
using MyBlogAPI.Services.LikeService;

namespace MyBlogAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly ILikeService _likeService;

        public CommentsController(ICommentService commentService, ILikeService likeService)
        {
            _commentService = commentService;
            _likeService = likeService;
        }

        [HttpGet("All")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _commentService.GetAllComments());
        }

        [HttpGet()]
        public async Task<IActionResult> GetComments(string sortingDirection = "ASC", string orderBy = null, int offset = 1, 
            int limit = 10, string authorUsername = null, string postParentName = null, string content = null)
        {
            var validFilter = new PaginationFilter(offset, limit);

            return Ok(await _commentService.GetComments(new CommentQueryFilter(authorUsername, postParentName, content).GetFilterSpecification(),
                new PagingSpecification((validFilter.Offset - 1) * validFilter.Limit, validFilter.Limit),
                new SortCommentFilter(sortingDirection, orderBy).GetSorting()));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _commentService.GetComment(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(AddCommentDto comment)
        {
            return Ok(await _commentService.AddComment(comment));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateComment(UpdateCommentDto comment)
        {
            if (await _commentService.GetComment(comment.Id) == null)
                return NotFound();
            await _commentService.UpdateComment(comment);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            if (await _commentService.GetComment(id) == null)
                return NotFound();
            await _commentService.DeleteComment(id);
            return Ok();
        }

        [HttpGet("{id:int}/Likes/")]
        public async Task<IActionResult> GetLikesFromComment(int id)
        {
            return Ok(await _likeService.GetLikesFromComment(id));
        }
    }
}
