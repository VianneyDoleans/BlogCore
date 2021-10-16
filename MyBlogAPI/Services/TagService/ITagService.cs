using System.Collections.Generic;
using System.Threading.Tasks;
using DbAccess.Data.POCO;
using DbAccess.Specifications;
using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.SortSpecification;
using MyBlogAPI.DTO.Tag;

namespace MyBlogAPI.Services.TagService
{
    public interface ITagService
    {
        Task<IEnumerable<GetTagDto>> GetAllTags();

        public Task<IEnumerable<GetTagDto>> GetTags(FilterSpecification<Tag> filterSpecification = null,
            PagingSpecification pagingSpecification = null,
            SortSpecification<Tag> sortSpecification = null);

        public Task<int> CountTagsWhere(FilterSpecification<Tag> filterSpecification = null);

        Task<GetTagDto> GetTag(int id);

        Task<GetTagDto> AddTag(AddTagDto user);

        Task UpdateTag(UpdateTagDto user);

        Task DeleteTag(int id);
    }
}
