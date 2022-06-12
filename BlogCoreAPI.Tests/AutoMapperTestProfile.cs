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
using DBAccess.Repositories.Category;
using DBAccess.Repositories.Comment;
using DBAccess.Repositories.Like;
using DBAccess.Repositories.Post;
using DBAccess.Repositories.Role;
using DBAccess.Repositories.Tag;
using DBAccess.Repositories.User;

namespace BlogCoreAPI.Tests
{
    public class AutoMapperTestProfile : Profile
    {
        public AutoMapperTestProfile(ILikeRepository likeRepository,
            IUserRepository userRepository,
            ICategoryRepository categoryRepository,
            ICommentRepository commentRepository,
            IRoleRepository roleRepository,
            IPostRepository postRepository,
            ITagRepository tagRepository)
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

            CreateMap<int, Like>().ConvertUsing(new LikeIdConverter(likeRepository));
            CreateMap<int, Comment>().ConvertUsing(new CommentIdConverter(commentRepository));
            CreateMap<int, Post> ().ConvertUsing(new PostIdConverter(postRepository));
            CreateMap<int, User>().ConvertUsing(new UserIdConverter(userRepository));
            CreateMap<int, Category>().ConvertUsing(new CategoryIdConverter(categoryRepository));
            CreateMap<int, Role>().ConvertUsing(new RoleIdConverter(roleRepository));
            CreateMap<int, Tag>().ConvertUsing(new TagIdConverter(tagRepository));

            CreateMap<UpdateCategoryDto, Category>().ConvertUsing(new UpdateCategoryConverter());
            CreateMap<UpdateUserDto, User>().ConvertUsing(new UpdateUserConverter());
            CreateMap<UpdateCommentDto, Comment>().ConvertUsing(new UpdateCommentConverter(commentRepository, postRepository, userRepository));
            CreateMap<UpdateLikeDto, Like>().ConvertUsing(new UpdateLikeConverter(commentRepository, postRepository, userRepository));
            CreateMap<UpdatePostDto, Post>().ConvertUsing(new UpdatePostConverter(userRepository, categoryRepository));

            CreateMap<Permission, PermissionDto>();
            CreateMap<PermissionAction, PermissionActionDto>().ConvertUsing(new PermissionActionConverter());
            CreateMap<PermissionRange, PermissionRangeDto>().ConvertUsing(new PermissionRangeConverter());
            CreateMap<PermissionTarget, PermissionTargetDto>().ConvertUsing(new PermissionTargetConverter());
        }
    }
}
