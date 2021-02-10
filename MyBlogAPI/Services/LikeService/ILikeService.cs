using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlogAPI.DTO;

namespace MyBlogAPI.Services.LikeService
{
    public interface ILikeService
    {
        ICollection<Like> GetAllLikes();

        Like GetLike(int id);

        void AddLike(Like user);

        void UpdateLike(Like user);

        void DeleteLike(int id);
    }
}
