using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DbAccess;
using DbAccess.Repositories.Category;
using DbAccess.Repositories.Comment;
using DbAccess.Repositories.Like;
using DbAccess.Repositories.Post;
using DbAccess.Repositories.Role;
using DbAccess.Repositories.Tag;
using DbAccess.Repositories.UnitOfWork;
using DbAccess.Repositories.User;
using Microsoft.OpenApi.Models;
using MyBlogAPI.Services.CategoryService;
using MyBlogAPI.Services.CommentService;
using MyBlogAPI.Services.LikeService;
using MyBlogAPI.Services.PostService;
using MyBlogAPI.Services.RoleService;
using MyBlogAPI.Services.TagService;
using MyBlogAPI.Services.UserService;

namespace MyBlogAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private static void ConfigureRepositoryServices(IServiceCollection services)
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ILikeRepository, LikeRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }

        private static void ConfigureServiceServices(IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ILikeService, LikeService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.RegisterDataServices(Configuration);
            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddSwaggerGen(c =>
                c.SwaggerDoc("v1", new OpenApiInfo() {Title = "MyBlog", Version = "v1"}));

            ConfigureRepositoryServices(services);
            ConfigureServiceServices(services);

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON Endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.    
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Blog V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
