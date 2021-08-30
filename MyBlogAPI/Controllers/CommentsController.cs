using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DbAccess.Specifications;
using MyBlogAPI.DTO.Comment;
using MyBlogAPI.Filters;
using MyBlogAPI.Filters.Category;
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

        /*[HttpGet()]
        public async Task<IActionResult> Get(string sortingDirection = "ASC", int pageNumber = 1, int pageSize = 10,
            string name = null, int? minimumPostNumber = null, int? maximumPostNumber = null)
        {
            var validFilter = new PaginationFilter(pageNumber, pageSize);

            return Ok(await _commentService.GetComments(new CategoryQueryFilter(name, minimumPostNumber, maximumPostNumber).GetFilterSpecification(),
                new PagingSpecification((validFilter.PageNumber - 1) * validFilter.PageSize, validFilter.PageSize),
                new SortCategoryFilter(sortingDirection).GetSorting()));
        }*/

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
