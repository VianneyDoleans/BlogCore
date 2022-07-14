using BlogCoreAPI.DTOs;
using BlogCoreAPI.DTOs.Tag;

namespace BlogCoreAPI.Models.DTOs.Tag
{
    /// <summary>
    /// GET Dto type of <see cref="Tag"/>.
    /// </summary>
    public class GetTagDto : ADto, ITagDto
    {
        public string Name { get; set; }
    }
}
