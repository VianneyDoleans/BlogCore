using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DbAccess;
using DbAccess.Data.POCO.Jwt;
using DbAccess.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MyBlogAPI.Extensions;
using MyBlogAPI.Permissions;
using MyBlogAPI.Services;
using MyBlogAPI.Services.JwtService;

namespace MyBlogAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.RegisterDatabaseProvider(Configuration);
            services.RegisterIdentityService();

            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MyBlog",
                    Version = "v1"
                });
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "MyBlogAPI.xml"));

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                }); var security =
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "Bearer",
                                    Type = ReferenceType.SecurityScheme
                                },
                                UnresolvedReference = true
                            },
                            new List<string>()
                        }
                    }; c.AddSecurityRequirement(security);
            });

            services.Configure<JwtSettings>(Configuration.GetSection("Jwt"));
            services.RegisterRepositoryServices();
            services.RegisterResourceServices();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            var jwtSettings = Configuration.GetSection("Jwt").Get<JwtSettings>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddAuth(jwtSettings);
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/error");
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            // Enable middleware to serve generated Swagger as a JSON Endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.    
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Blog API V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuth();
            //app.UseAuthentication();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
