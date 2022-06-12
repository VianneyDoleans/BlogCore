using AutoMapper;
using DBAccess.Data.POCO;

namespace BlogCoreAPI.DTOs.Category.Converters
{
    /// <summary>
    /// AutoMapper converter used to enable the conversion of <see cref="UpdateCategoryDto"/> to <see cref="Category"/>.
    /// </summary>
    public class UpdateCategoryConverter : ITypeConverter<UpdateCategoryDto, DBAccess.Data.POCO.Category>
    {
        /// <inheritdoc />
        public DBAccess.Data.POCO.Category Convert(UpdateCategoryDto source, DBAccess.Data.POCO.Category destination,
            ResolutionContext context)
        {
            destination.Name = source.Name;
            return destination;
        }
    }
}
