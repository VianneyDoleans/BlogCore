namespace MyBlogAPI.DTO.Tag
{
    public class UpdateTagDto : ADto, ITagDto
    {
        public string Name { get; set; }
    }
}
