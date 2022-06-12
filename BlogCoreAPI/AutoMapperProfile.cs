using AutoMapper;
using BlogCoreAPI.DTOs.Category;
using BlogCoreAPI.DTOs.Category.Converters;
using BlogCoreAPI.DTOs.Comment;
using BlogCoreAPI.DTOs.Comment.Converters;
using BlogCoreAPI.DTOs.Like;
using BlogCoreAPI.DTOs.Like.Converters;
using BlogCoreAPI.DTOs.Permission;
using BlogCoreAPI.DTOs.Permission.Converters;
using BlogCoreAPI.DTOs.Post;
using BlogCoreAPI.DTOs.Post.Converters;
using BlogCoreAPI.DTOs.Role;
using BlogCoreAPI.DTOs.Role.Converters;
using BlogCoreAPI.DTOs.Tag;
using BlogCoreAPI.DTOs.Tag.Converters;
using BlogCoreAPI.DTOs.User;
using BlogCoreAPI.DTOs.User.Converters;
using DBAccess.Data.POCO;
using DBAccess.Data.POCO.Permission;

namespace BlogCoreAPI
{
    /// <inheritdoc />
    public class AutoMapperProfile : Profile
    {
        /// <inheritdoc />
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
            CreateMap<UpdatePostDto, Post>().ConvertUsing<UpdatePostConverter>();

            CreateMap<Permission, PermissionDto>();
            CreateMap<PermissionAction, PermissionActionDto>().ConvertUsing(new PermissionActionConverter());
            CreateMap<PermissionRange, PermissionRangeDto>().ConvertUsing(new PermissionRangeConverter());
            CreateMap<PermissionTarget, PermissionTargetDto>().ConvertUsing(new PermissionTargetConverter());
        }
    }
}
