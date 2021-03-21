using AutoMapper;

namespace MyBlogAPI.DTO.Category.Converters
{
    public class UpdateCategoryConverter : ITypeConverter<UpdateCategoryDto, DbAccess.Data.POCO.Category>
    {
        public DbAccess.Data.POCO.Category Convert(UpdateCategoryDto source, DbAccess.Data.POCO.Category destination,
            ResolutionContext context)
        {
            destination.Name = source.Name;
            return destination;
        }
    }
}
