namespace MyBlogAPI.DTOs.Tag
{
    /// <summary>
    /// Add Dto type of <see cref="DbAccess.Data.POCO.Tag"/>.
    /// </summary>
    public class AddTagDto : ITagDto
    {
        public string Name { get; set; }
    }
}
