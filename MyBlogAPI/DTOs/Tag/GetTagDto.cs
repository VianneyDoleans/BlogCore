namespace MyBlogAPI.DTO.Tag
{
    /// <summary>
    /// GET Dto type of <see cref="DbAccess.Data.POCO.Tag"/>.
    /// </summary>
    public class GetTagDto : ADto, ITagDto
    {
        public string Name { get; set; }
    }
}
