using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DbAccess.Data.POCO;
using DbAccess.Specifications;
using MyBlogAPI.DTO.Like;
using MyBlogAPI.Filters;
using MyBlogAPI.Filters.Like;
using MyBlogAPI.Services.LikeService;

namespace MyBlogAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LikesController : ControllerBase
    {
        private readonly ILikeService _likeService;

        public LikesController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _likeService.GetAllLikes());
        }

        [HttpGet()]
        public async Task<IActionResult> GetLikes(string sortingDirection = "ASC", int pageNumber = 1,
            int pageSize = 10, LikeableType? likeableType = null)
        {
            var validFilter = new PaginationFilter(pageNumber, pageSize);

            return Ok(await _likeService.GetLikes(new LikeQueryFilter(likeableType).GetFilterSpecification(),
                new PagingSpecification((validFilter.PageNumber - 1) * validFilter.PageSize, validFilter.PageSize),
                new SortLikeFilter(sortingDirection).GetSorting()));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _likeService.GetLike(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddLikes(AddLikeDto like)
        {
            return Ok(await _likeService.AddLike(like));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateLike(UpdateLikeDto like)
        {
            if (await _likeService.GetLike(like.Id) == null)
                return NotFound();
            await _likeService.UpdateLike(like);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteLike(int id)
        {
            if (await _likeService.GetLike(id) == null)
                return NotFound();
            await _likeService.DeleteLike(id);
            return Ok();
        }
    }
}
