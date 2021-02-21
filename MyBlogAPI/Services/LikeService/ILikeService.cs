using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlogAPI.DTO;
using MyBlogAPI.DTO.Like;

namespace MyBlogAPI.Services.LikeService
{
    public interface ILikeService
    {
        Task<IEnumerable<GetLikeDto>> GetAllLikes();

        Task<GetLikeDto> GetLike(int id);

        Task<GetLikeDto> AddLike(AddLikeDto user);

        Task UpdateLike(UpdateLikeDto like);

        Task DeleteLike(int id);

        Task<IEnumerable<GetLikeDto>> GetLikesFromUser(int id);

        Task<IEnumerable<GetLikeDto>> GetLikesFromPost(int id);

        Task<IEnumerable<GetLikeDto>> GetLikesFromComment(int id);
    }
}
