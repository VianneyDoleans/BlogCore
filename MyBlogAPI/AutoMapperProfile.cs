using DbAccess.Data.POCO;
using AutoMapper;
using MyBlogAPI.DTO.User;

namespace MyBlogAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, GetUserDto>();
            CreateMap<AddUserDto, User>();
        }
    }
}
