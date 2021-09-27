using AutoMapper;

namespace MyBlogAPI.DTO.User.Converters
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
            destination.EmailAddress = source.EmailAddress;
            destination.Username = source.Username;
            destination.Password = source.Password;
            return destination;
        }
    }
}
