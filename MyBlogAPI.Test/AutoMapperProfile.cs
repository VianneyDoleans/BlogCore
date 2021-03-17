using AutoMapper;
using DbAccess.Data.POCO;
using DbAccess.Repositories.Category;
using DbAccess.Repositories.Comment;
using DbAccess.Repositories.Like;
using DbAccess.Repositories.Post;
using DbAccess.Repositories.Role;
using DbAccess.Repositories.Tag;
using DbAccess.Repositories.User;
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

namespace MyBlogAPI.Test
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile(ILikeRepository likeRepository,
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
        }
    }
}
