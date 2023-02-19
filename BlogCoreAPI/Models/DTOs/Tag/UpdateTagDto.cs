namespace BlogCoreAPI.Models.DTOs.Tag
{
    /// <summary>
    /// UPDATE Dto type of <see cref="Tag"/>.
    /// </summary>
    public class UpdateTagDto : ADto, ITagDto
    {
        public string Name { get; set; }
    }
}
