using System.Collections.Generic;
using System.Threading.Tasks;
using BlogCoreAPI.Models.DTOs.Tag;
using DBAccess.Data;
using DBAccess.Specifications;
using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.SortSpecification;

namespace BlogCoreAPI.Services.TagService
{
    public interface ITagService
    {
        Task<IEnumerable<GetTagDto>> GetAllTags();

        public Task<IEnumerable<GetTagDto>> GetTags(FilterSpecification<Tag> filterSpecification = null,
            PagingSpecification pagingSpecification = null,
            SortSpecification<Tag> sortSpecification = null);

        public Task<int> CountTagsWhere(FilterSpecification<Tag> filterSpecification = null);

        Task<GetTagDto> GetTag(int id);

        Task<GetTagDto> AddTag(AddTagDto tag);

        Task UpdateTag(UpdateTagDto tag);

        Task DeleteTag(int id);
    }
}
