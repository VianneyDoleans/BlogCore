namespace MyBlogAPI.DTO.Category
{
    public class UpdateCategoryDto : ADto, ICategoryDto
    {
        public string Name { get; set; }
    }
}
