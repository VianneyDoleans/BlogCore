using AutoMapper;
using DbAccess.Data.POCO;
using DbAccess.Data.POCO.Permission;
using DbAccess.Repositories.Category;
using DbAccess.Repositories.Comment;
using DbAccess.Repositories.Like;
using DbAccess.Repositories.Post;
using DbAccess.Repositories.Role;
using DbAccess.Repositories.Tag;
using DbAccess.Repositories.User;
using MyBlogAPI.DTOs.Category;
using MyBlogAPI.DTOs.Category.Converters;
using MyBlogAPI.DTOs.Comment;
using MyBlogAPI.DTOs.Comment.Converters;
using MyBlogAPI.DTOs.Like;
using MyBlogAPI.DTOs.Like.Converters;
using MyBlogAPI.DTOs.Permission;
using MyBlogAPI.DTOs.Permission.Converters;
using MyBlogAPI.DTOs.Post;
using MyBlogAPI.DTOs.Post.Converters;
using MyBlogAPI.DTOs.Role;
using MyBlogAPI.DTOs.Role.Converters;
using MyBlogAPI.DTOs.Tag;
using MyBlogAPI.DTOs.Tag.Converters;
using MyBlogAPI.DTOs.User;
using MyBlogAPI.DTOs.User.Converters;

namespace MyBlogAPI.Tests
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
