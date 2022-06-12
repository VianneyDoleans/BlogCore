using System.Collections.Generic;
using System.Threading.Tasks;
using BlogCoreAPI.DTOs.Like;
using DBAccess.Data.POCO;
using DBAccess.Specifications;
using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.SortSpecification;

namespace BlogCoreAPI.Services.LikeService
{
    public interface ILikeService
    {
        Task<IEnumerable<GetLikeDto>> GetAllLikes();

        public Task<IEnumerable<GetLikeDto>> GetLikes(FilterSpecification<Like> filterSpecification = null,
            PagingSpecification pagingSpecification = null,
            SortSpecification<Like> sortSpecification = null);

        public Task<int> CountLikesWhere(FilterSpecification<Like> filterSpecification = null);

        Task<GetLikeDto> GetLike(int id);

        Task<GetLikeDto> AddLike(AddLikeDto like);

        Task UpdateLike(UpdateLikeDto like);

        Task DeleteLike(int id);

        Task<IEnumerable<GetLikeDto>> GetLikesFromUser(int id);

        Task<IEnumerable<GetLikeDto>> GetLikesFromPost(int id);

        Task<IEnumerable<GetLikeDto>> GetLikesFromComment(int id);
    }
}
