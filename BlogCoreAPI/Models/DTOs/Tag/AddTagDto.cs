namespace BlogCoreAPI.DTOs.Tag
{
    /// <summary>
    /// Add Dto type of <see cref="Tag"/>.
    /// </summary>
    public class AddTagDto : ITagDto
    {
        public string Name { get; set; }
    }
}
