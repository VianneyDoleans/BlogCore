using DbAccess.Repositories.Category;
using DbAccess.Repositories.Comment;
using DbAccess.Repositories.Like;
using DbAccess.Repositories.Post;
using DbAccess.Repositories.Role;
using DbAccess.Repositories.Tag;
using DbAccess.Repositories.User;
using Microsoft.Extensions.DependencyInjection;
using MyBlogAPI.Services.CategoryService;
using MyBlogAPI.Services.CommentService;
using MyBlogAPI.Services.LikeService;
using MyBlogAPI.Services.PostService;
using MyBlogAPI.Services.RoleService;
using MyBlogAPI.Services.TagService;
using MyBlogAPI.Services.UserService;

namespace MyBlogAPI.Services
{
    /// <summary> 
    /// Extension of <see cref="IServiceCollection"/> adding methods to inject MyBlog Services 
    /// </summary> 
    public static class ServiceCollectionExtensions
    {
        /// <summary> 
        /// class used to register repository services 
        /// </summary> 
        /// <param name="services"></param> 
        /// <returns></returns> 
        public static IServiceCollection RegisterRepositoryServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ILikeRepository, LikeRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }

        /// <summary> 
        /// class used to register resource services 
        /// </summary> 
        /// <param name="services"></param> 
        /// <returns></returns> 
        public static IServiceCollection RegisterResourceServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService.CategoryService>();
            services.AddScoped<ICommentService, CommentService.CommentService>();
            services.AddScoped<ILikeService, LikeService.LikeService>();
            services.AddScoped<IPostService, PostService.PostService>();
            services.AddScoped<IRoleService, RoleService.RoleService>();
            services.AddScoped<ITagService, TagService.TagService>();
            services.AddScoped<IUserService, UserService.UserService>();
            return services;
        }
    }
}
