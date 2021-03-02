using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlogAPI.DTO;
using MyBlogAPI.DTO.Comment;

namespace MyBlogAPI.Services.CommentService
{
    public interface ICommentService
    {
        Task<IEnumerable<GetCommentDto>> GetAllComments();

        Task<GetCommentDto> GetComment(int id);

        Task<GetCommentDto> AddComment(AddCommentDto comment);

        Task UpdateComment(UpdateCommentDto comment);

        Task DeleteComment(int id);

        Task<IEnumerable<GetCommentDto>> GetCommentsFromUser(int id);

        Task<IEnumerable<GetCommentDto>> GetCommentsFromPost(int id);
    }
}
