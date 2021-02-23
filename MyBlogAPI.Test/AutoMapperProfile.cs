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
using MyBlogAPI.DTO.Comment;
using MyBlogAPI.DTO.Like;
using MyBlogAPI.DTO.Post;
using MyBlogAPI.DTO.Role;
using MyBlogAPI.DTO.Tag;
using MyBlogAPI.DTO.User;

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

            CreateMap<int, Like>().ConvertUsing(new LikeDtoConverter(likeRepository));
            CreateMap<int, Comment>().ConvertUsing(new CommentDtoConverter(commentRepository));
            CreateMap<int, Post> ().ConvertUsing(new PostDtoConverter(postRepository));
            CreateMap<int, User>().ConvertUsing(new UserDtoConverter(userRepository));
            CreateMap<int, Category>().ConvertUsing(new CategoryDtoConverter(categoryRepository));
            CreateMap<int, Role>().ConvertUsing(new RoleDtoConverter(roleRepository));
            CreateMap<int, Tag>().ConvertUsing(new TagDtoConverter(tagRepository));
        }
    }
}
