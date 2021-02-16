using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlogAPI.DTO;
using MyBlogAPI.DTO.Post;

namespace MyBlogAPI.Services.PostService
{
    public interface IPostService
    {
        Task<IEnumerable<GetPostDto>> GetAllPosts();

        Task<IEnumerable<GetPostDto>> GetPostsFromUser(int id);

        Task<IEnumerable<GetPostDto>> GetPostsFromTag(int id);

        Task<IEnumerable<GetPostDto>> GetPostsFromCategory(int id);

        Task<GetPostDto> GetPost(int id);

        Task AddPost(AddPostDto post);

        Task UpdatePost(AddPostDto post);

        Task DeletePost(int id);
    }
}
