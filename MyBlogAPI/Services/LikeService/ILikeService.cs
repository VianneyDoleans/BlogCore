using System.Collections.Generic;
using System.Threading.Tasks;
using DbAccess.Data.POCO;
using DbAccess.Specifications;
using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.SortSpecification;
using MyBlogAPI.DTO.Like;

namespace MyBlogAPI.Services.LikeService
{
    public interface ILikeService
    {
        Task<IEnumerable<GetLikeDto>> GetAllLikes();

        public Task<IEnumerable<GetLikeDto>> GetLikes(FilterSpecification<Like> filterSpecification = null,
            PagingSpecification pagingSpecification = null,
            SortSpecification<Like> sortSpecification = null);

        public Task<int> CountLikesWhere(FilterSpecification<Like> filterSpecification = null);

        Task<GetLikeDto> GetLike(int id);

        Task<Like> GetLikeEntity(int id);

        Task<GetLikeDto> AddLike(AddLikeDto user);

        Task UpdateLike(UpdateLikeDto like);

        Task DeleteLike(int id);

        Task<IEnumerable<GetLikeDto>> GetLikesFromUser(int id);

        Task<IEnumerable<GetLikeDto>> GetLikesFromPost(int id);

        Task<IEnumerable<GetLikeDto>> GetLikesFromComment(int id);
    }
}
