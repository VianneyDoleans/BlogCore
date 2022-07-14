using AutoMapper;

namespace BlogCoreAPI.DTOs.Category.Converters
{
    /// <summary>
    /// AutoMapper converter used to enable the conversion of <see cref="UpdateCategoryDto"/> to <see cref="Category"/>.
    /// </summary>
    public class UpdateCategoryConverter : ITypeConverter<UpdateCategoryDto, DBAccess.Data.Category>
    {
        /// <inheritdoc />
        public DBAccess.Data.Category Convert(UpdateCategoryDto source, DBAccess.Data.Category destination,
            ResolutionContext context)
        {
            destination.Name = source.Name;
            return destination;
        }
    }
}
