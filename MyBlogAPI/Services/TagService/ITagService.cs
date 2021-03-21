using System.Collections.Generic;
using System.Threading.Tasks;
using MyBlogAPI.DTO.Tag;

namespace MyBlogAPI.Services.TagService
{
    public interface ITagService
    {
        Task<IEnumerable<GetTagDto>> GetAllTags();

        Task<GetTagDto> GetTag(int id);

        Task<GetTagDto> AddTag(AddTagDto user);

        Task UpdateTag(UpdateTagDto user);

        Task DeleteTag(int id);
    }
}
