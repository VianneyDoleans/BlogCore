using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlogAPI.DTO.Comment;
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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _commentService.GetAllComments());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _commentService.GetComment(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(AddCommentDto comment)
        {
            await _commentService.AddComment(comment);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateComment(AddCommentDto comment)
        {
            if (await _commentService.GetComment(comment.Id) == null)
                return NotFound();
            await _commentService.UpdateComment(comment);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            if (await _commentService.GetComment(id) == null)
                return NotFound();
            await _commentService.DeleteComment(id);
            return Ok();
        }

        [HttpGet("{id}/Likes/")]
        public async Task<IActionResult> GetLikesFromComment(int id)
        {
            return Ok(await _likeService.GetLikesFromComment(id));
        }
    }
}
