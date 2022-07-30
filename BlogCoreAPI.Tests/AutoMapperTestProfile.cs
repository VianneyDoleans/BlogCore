using AutoMapper;
using BlogCoreAPI.Models.DTOs.Category;
using BlogCoreAPI.Models.DTOs.Category.Converters;
using BlogCoreAPI.Models.DTOs.Comment;
using BlogCoreAPI.Models.DTOs.Comment.Converters;
using BlogCoreAPI.Models.DTOs.Like;
using BlogCoreAPI.Models.DTOs.Like.Converters;
using BlogCoreAPI.Models.DTOs.Permission;
using BlogCoreAPI.Models.DTOs.Permission.Converters;
using BlogCoreAPI.Models.DTOs.Post;
using BlogCoreAPI.Models.DTOs.Post.Converters;
using BlogCoreAPI.Models.DTOs.Role;
using BlogCoreAPI.Models.DTOs.Role.Converters;
using BlogCoreAPI.Models.DTOs.Tag;
using BlogCoreAPI.Models.DTOs.Tag.Converters;
using BlogCoreAPI.Models.DTOs.User;
using BlogCoreAPI.Models.DTOs.User.Converters;
using DBAccess.Data;
using DBAccess.Data.Permission;
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
