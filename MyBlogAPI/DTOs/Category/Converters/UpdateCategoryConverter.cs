using AutoMapper;

namespace MyBlogAPI.DTOs.Category.Converters
{
    /// <summary>
    /// AutoMapper converter used to enable the conversion of <see cref="UpdateCategoryDto"/> to <see cref="DbAccess.Data.POCO.Category"/>.
    /// </summary>
    public class UpdateCategoryConverter : ITypeConverter<UpdateCategoryDto, DbAccess.Data.POCO.Category>
    {
        /// <inheritdoc />
        public DbAccess.Data.POCO.Category Convert(UpdateCategoryDto source, DbAccess.Data.POCO.Category destination,
            ResolutionContext context)
        {
            destination.Name = source.Name;
            return destination;
        }
    }
}
