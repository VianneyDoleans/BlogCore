using AutoMapper;

namespace MyBlogAPI.DTOs.User.Converters
{
    /// <summary>
    /// AutoMapper converter used to enable the conversion of <see cref="UpdateUserDto"/> to <see cref="DbAccess.Data.POCO.User"/>.
    /// </summary>
    public class UpdateUserConverter : ITypeConverter<UpdateUserDto, DbAccess.Data.POCO.User>
    {
        /// <inheritdoc />
        public DbAccess.Data.POCO.User Convert(UpdateUserDto source, DbAccess.Data.POCO.User destination,
            ResolutionContext context)
        {
            destination.UserDescription = source.UserDescription;
            destination.Email = source.Email;
            destination.UserName = source.UserName;
            destination.Password = source.Password;
            return destination;
        }
    }
}
