using System.Collections.Generic;
using System.Threading.Tasks;
using DbAccess.Data.POCO;
using DbAccess.Specifications;
using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.SortSpecification;
using MyBlogAPI.DTOs.Post;

namespace MyBlogAPI.Services.PostService
{
    public interface IPostService
    {
        Task<IEnumerable<GetPostDto>> GetAllPosts();

        public Task<IEnumerable<GetPostDto>> GetPosts(FilterSpecification<Post> filterSpecification = null,
            PagingSpecification pagingSpecification = null,
            SortSpecification<Post> sortSpecification = null);

        public Task<int> CountPostsWhere(FilterSpecification<Post> filterSpecification = null);

        Task<IEnumerable<GetPostDto>> GetPostsFromUser(int id);

        Task<IEnumerable<GetPostDto>> GetPostsFromTag(int id);

        Task<IEnumerable<GetPostDto>> GetPostsFromCategory(int id);

        Task<GetPostDto> GetPost(int id);

        Task<GetPostDto> AddPost(AddPostDto post);

        Task UpdatePost(UpdatePostDto post);

        Task DeletePost(int id);
    }
}
