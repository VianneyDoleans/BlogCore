namespace MyBlogAPI.DTOs.Tag
{
    /// <summary>
    /// Interface of <see cref="DbAccess.Data.POCO.Tag"/> Dto containing all the common properties of Tag Dto Type (GET, ADD, UPDATE).
    /// </summary>
    public interface ITagDto
    {
        public string Name { get; set; }
    }
}
