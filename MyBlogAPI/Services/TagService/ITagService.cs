using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlogAPI.DTO;
using MyBlogAPI.DTO.Tag;

namespace MyBlogAPI.Services.TagService
{
    public interface ITagService
    {
        Task<IEnumerable<GetTagDto>> GetAllTags();

        Task<GetTagDto> GetTag(int id);

        Task AddTag(AddTagDto user);

        Task UpdateTag(AddTagDto user);

        Task DeleteTag(int id);
    }
}
