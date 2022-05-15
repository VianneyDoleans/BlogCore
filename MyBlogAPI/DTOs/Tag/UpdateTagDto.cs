namespace MyBlogAPI.DTOs.Tag
{
    /// <summary>
    /// UPDATE Dto type of <see cref="DbAccess.Data.POCO.Tag"/>.
    /// </summary>
    public class UpdateTagDto : ADto, ITagDto
    {
        public string Name { get; set; }
    }
}
