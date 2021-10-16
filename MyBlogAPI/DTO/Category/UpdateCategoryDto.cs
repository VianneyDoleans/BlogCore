namespace MyBlogAPI.DTO.Category
{
    /// <summary>
    /// UPDATE Dto type of <see cref="DbAccess.Data.POCO.Category"/>.
    /// </summary>
    public class UpdateCategoryDto : ADto, ICategoryDto
    {
        public string Name { get; set; }
    }
}
