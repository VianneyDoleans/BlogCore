using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlogAPI.DTO.Like;
using MyBlogAPI.DTO.User;
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

        [HttpGet("{id}")]
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLike(int id)
        {
            if (await _likeService.GetLike(id) == null)
                return NotFound();
            await _likeService.DeleteLike(id);
            return Ok();
        }
    }
}
