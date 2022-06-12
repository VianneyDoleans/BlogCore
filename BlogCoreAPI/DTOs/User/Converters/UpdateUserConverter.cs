using AutoMapper;
using DBAccess.Data.POCO;

namespace BlogCoreAPI.DTOs.User.Converters
{
    /// <summary>
    /// AutoMapper converter used to enable the conversion of <see cref="UpdateUserDto"/> to <see cref="User"/>.
    /// </summary>
    public class UpdateUserConverter : ITypeConverter<UpdateUserDto, DBAccess.Data.POCO.User>
    {
        /// <inheritdoc />
        public DBAccess.Data.POCO.User Convert(UpdateUserDto source, DBAccess.Data.POCO.User destination,
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
