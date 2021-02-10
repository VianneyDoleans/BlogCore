using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlogAPI.DTO;

namespace MyBlogAPI.Services.CommentService
{
    public interface ICommentService
    {
        ICollection<Comment> GetAllComments();

        Comment GetComment(int id);

        void AddComment(Comment comment);

        void UpdateComment(Comment comment);

        void DeleteComment(int id);
    }
}
