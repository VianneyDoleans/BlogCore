using BlogCoreAPI.Authorization.PermissionHandlers.Attributes;
using BlogCoreAPI.Authorization.PermissionHandlers.Dtos;
using BlogCoreAPI.Authorization.PermissionHandlers.Resources;
using BlogCoreAPI.Models.DTOs.Category;
using BlogCoreAPI.Models.DTOs.Comment;
using BlogCoreAPI.Models.DTOs.Like;
using BlogCoreAPI.Models.DTOs.Post;
using BlogCoreAPI.Models.DTOs.Role;
using BlogCoreAPI.Models.DTOs.Tag;
using BlogCoreAPI.Models.DTOs.User;
using BlogCoreAPI.Services.CategoryService;
using BlogCoreAPI.Services.CommentService;
using BlogCoreAPI.Services.LikeService;
using BlogCoreAPI.Services.PostService;
using BlogCoreAPI.Services.RoleService;
using BlogCoreAPI.Services.TagService;
using BlogCoreAPI.Services.UserService;
using BlogCoreAPI.Validators.Category;
using BlogCoreAPI.Validators.Comment;
using BlogCoreAPI.Validators.Like;
using BlogCoreAPI.Validators.Post;
using BlogCoreAPI.Validators.Role;
using BlogCoreAPI.Validators.Tag;
using BlogCoreAPI.Validators.User;
using DBAccess.Data;
using DBAccess.Repositories.Category;
using DBAccess.Repositories.Comment;
using DBAccess.Repositories.Like;
using DBAccess.Repositories.Post;
using DBAccess.Repositories.Role;
using DBAccess.Repositories.Tag;
using DBAccess.Repositories.UnitOfWork;
using DBAccess.Repositories.User;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace BlogCoreAPI.Extensions
{
    /// <summary> 
    /// Extension of <see cref="IServiceCollection"/> adding methods to inject BlogCoreAPI Services 
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
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        /// <summary> 
        /// class used to register resource services 
        /// </summary> 
        /// <param name="services"></param> 
        /// <returns></returns> 
        public static IServiceCollection RegisterResourceServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ILikeService, LikeService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }

        /// <summary> 
        /// class used to register resource validators
        /// </summary> 
        /// <param name="services"></param> 
        /// <returns></returns> 
        public static IServiceCollection RegisterDtoResourceValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<ICategoryDto>, CategoryDtoValidator>();
            services.AddScoped<IValidator<ICommentDto>, CommentDtoValidator>();
            services.AddScoped<IValidator<ILikeDto>, LikeDtoValidator>();
            services.AddScoped<IValidator<IPostDto>, PostDtoValidator>();
            services.AddScoped<IValidator<IRoleDto>, RoleDtoValidator>();
            services.AddScoped<IValidator<ITagDto>, TagDtoValidator>();
            services.AddScoped<IValidator<IUserDto>, UserDtoValidator>();
            return services;
        }

        /// <summary> 
        /// class used to register resource services 
        /// </summary> 
        /// <param name="services"></param> 
        /// <returns></returns> 
        public static IServiceCollection RegisterAuthorizationHandlers(this IServiceCollection services)
        {
            // Resource Handlers
            services.AddScoped<IAuthorizationHandler, HasAllPermissionRangeAuthorizationHandler<Role>>();
            services.AddScoped<IAuthorizationHandler, HasAllPermissionRangeAuthorizationHandler<Tag>>();
            services.AddScoped<IAuthorizationHandler, HasAllPermissionRangeAuthorizationHandler<Category>>();
            services.AddScoped<IAuthorizationHandler, HasOwnOrAllPermissionRangeForHasAuthorEntityAuthorizationHandler<Comment>>();
            services.AddScoped<IAuthorizationHandler, HasOwnOrAllPermissionRangeForHasAuthorEntityAuthorizationHandler<Post>>();
            services.AddScoped<IAuthorizationHandler, HasOwnOrAllPermissionRangeForHasUserEntityAuthorizationHandler<Like>>();
            services.AddScoped<IAuthorizationHandler, HasOwnOrAllPermissionRangeForUserResourceAuthorizationHandler>();

            // DTO Handlers
            services.AddScoped<IAuthorizationHandler, HasOwnOrAllPermissionRangeForHasAuthorDtoAuthorizationHandler<ICommentDto>>();
            services.AddScoped<IAuthorizationHandler, HasOwnOrAllPermissionRangeForHasAuthorDtoAuthorizationHandler<IPostDto>>();
            services.AddScoped<IAuthorizationHandler, HasOwnOrAllPermissionRangeForHasUserDtoAuthorizationHandler<ILikeDto>>();

            // Resource Attribute Handler
            services.AddScoped<IAuthorizationHandler, PermissionWithRangeAuthorizationHandler>();
            
            return services;
        }
    }
}
