using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DbAccess.Data.POCO;
using DbAccess.Specifications;
using MyBlogAPI.DTO.Like;
using MyBlogAPI.Filters;
using MyBlogAPI.Filters.Like;
using MyBlogAPI.Responses;
using MyBlogAPI.Services.LikeService;

namespace MyBlogAPI.Controllers
{
    /// <summary>
    /// Controller used to expose Like resources of the API.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class LikesController : ControllerBase
    {
        private readonly ILikeService _likeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LikesController"/> class.
        /// </summary>
        /// <param name="likeService"></param>
        public LikesController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        /// <summary>
        /// Get list of likes.
        /// </summary>
        /// <remarks>
        /// Get list of likes. The endpoint uses pagination and sort. Filter(s) can be applied for research.
        /// </remarks>
        /// <param name="sortingDirection"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="likeableType"></param>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> GetLikes(string sortingDirection = "ASC", int page = 1,
            int size = 10, LikeableType? likeableType = null)
        {
            var validPagination = new PaginationFilter(page, size);
            var filterSpecification = new LikeQueryFilter(likeableType).GetFilterSpecification();
            var data = await _likeService.GetLikes(filterSpecification,
                new PagingSpecification((validPagination.Page - 1) * validPagination.Limit, validPagination.Limit),
                new SortLikeFilter(sortingDirection).GetSorting());

            return Ok(new PagedBlogResponse<GetLikeDto>(data, validPagination.Page, validPagination.Limit,
                await _likeService.CountLikesWhere(filterSpecification)));
        }

        /// <summary>
        /// Get a like by giving its Id.
        /// </summary>
        /// <remarks>
        /// Get a like by giving its Id.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _likeService.GetLike(id));
        }

        /// <summary>
        /// Add a like.
        /// </summary>
        /// <remarks>
        /// Add a like.
        /// </remarks>
        /// <param name="like"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddLikes(AddLikeDto like)
        {
            return Ok(await _likeService.AddLike(like));
        }

        /// <summary>
        /// Update a like.
        /// </summary>
        /// <remarks>
        /// Update a like.
        /// </remarks>
        /// <param name="like"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateLike(UpdateLikeDto like)
        {
            if (await _likeService.GetLike(like.Id) == null)
                return NotFound();
            await _likeService.UpdateLike(like);
            return Ok();
        }

        /// <summary>
        /// Delete a like by giving its id.
        /// </summary>
        /// <remarks>
        /// Delete a like by giving its id.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
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
