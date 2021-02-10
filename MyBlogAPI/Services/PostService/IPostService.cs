using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlogAPI.DTO;

namespace MyBlogAPI.Services.PostService
{
    public interface IPostService
    {
        ICollection<Post> GetAllPosts();

        Post GetPost(int id);

        void AddPost(Post post);

        void UpdatePost(Post post);

        void DeletePost(int id);
    }
}
