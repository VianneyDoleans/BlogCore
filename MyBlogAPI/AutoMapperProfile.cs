using DbAccess.Data.POCO;
using AutoMapper;
using MyBlogAPI.DTO.Category;
using MyBlogAPI.DTO.Category.Converters;
using MyBlogAPI.DTO.Comment;
using MyBlogAPI.DTO.Comment.Converters;
using MyBlogAPI.DTO.Like;
using MyBlogAPI.DTO.Like.Converters;
using MyBlogAPI.DTO.Post;
using MyBlogAPI.DTO.Post.Converters;
using MyBlogAPI.DTO.Role;
using MyBlogAPI.DTO.Role.Converters;
using MyBlogAPI.DTO.Tag;
using MyBlogAPI.DTO.Tag.Converters;
using MyBlogAPI.DTO.User;
using MyBlogAPI.DTO.User.Converters;

namespace MyBlogAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddCategoryDto, Category>();
            CreateMap<AddCommentDto, Comment>();
            CreateMap<AddLikeDto, Like>();
            CreateMap<AddPostDto, Post>();
            CreateMap<AddRoleDto, Role>();
            CreateMap<AddTagDto, Tag>();
            CreateMap<AddUserDto, User>();

            CreateMap<Category, GetCategoryDto>();
            CreateMap<Comment, GetCommentDto>();
            CreateMap<Like, GetLikeDto>();
            CreateMap<Post, GetPostDto>();
            CreateMap<Role, GetRoleDto>();
            CreateMap<Tag, GetTagDto>();
            CreateMap<User, GetUserDto>();

            CreateMap<GetCategoryDto, UpdateCategoryDto>();
            CreateMap<GetCommentDto, UpdateCommentDto>();
            CreateMap<GetLikeDto, UpdateLikeDto>();
            CreateMap<GetPostDto, UpdatePostDto>();
            CreateMap<GetRoleDto, UpdateRoleDto>();
            CreateMap<GetTagDto, UpdateTagDto>();
            CreateMap<GetUserDto, UpdateUserDto>();

            CreateMap<Like, int>().ConvertUsing(x => x.Id);
            CreateMap<Comment, int>().ConvertUsing(x => x.Id);
            CreateMap<Post, int>().ConvertUsing(x => x.Id);
            CreateMap<User, int>().ConvertUsing(x => x.Id);
            CreateMap<Category, int>().ConvertUsing(x => x.Id);
            CreateMap<Role, int>().ConvertUsing(x => x.Id);
            CreateMap<Tag, int>().ConvertUsing(x => x.Id);

            CreateMap<int, Like>().ConvertUsing<LikeIdConverter>();
            CreateMap<int, Comment>().ConvertUsing<CommentIdConverter>();
            CreateMap<int, Post> ().ConvertUsing<PostIdConverter>();
            CreateMap<int, User>().ConvertUsing<UserIdConverter>();
            CreateMap<int, Category>().ConvertUsing<CategoryIdConverter>();
            CreateMap<int, Role>().ConvertUsing<RoleIdConverter>();
            CreateMap<int, Tag>().ConvertUsing<TagIdConverter>();

            CreateMap<UpdateCategoryDto, Category>().ConvertUsing<UpdateCategoryConverter>();
            CreateMap<UpdateUserDto, User>().ConvertUsing(new UpdateUserConverter());
            CreateMap<UpdateCommentDto, Comment>().ConvertUsing<UpdateCommentConverter>();
            CreateMap<UpdateLikeDto, Like>().ConvertUsing<UpdateLikeConverter>();
        }
    }
}
