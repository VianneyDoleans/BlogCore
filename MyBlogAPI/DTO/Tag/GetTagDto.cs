namespace MyBlogAPI.DTO.Tag
{
    public class GetTagDto : ADto, ITagDto
    {
        public string Name { get; set; }

        /*public virtual IEnumerable<int> Posts { get; set; }*/
    }
}
