using System.Text.Json.Serialization;
using BlogCoreAPI;
using BlogCoreAPI.Extensions;
using BlogCoreAPI.Services.JwtService;
using DBAccess;
using DBAccess.Data;
using DBAccess.DataContext;
using DBAccess.Extensions;
using DBAccess.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Host.UseSerilog((_, lc) => lc
    .ReadFrom.Configuration(configuration)
    .Enrich.FromLogContext());

builder.Host.RegisterLogger(configuration);
builder.Services.RegisterFluentValidation();
builder.Services.RegisterDatabaseProvider(configuration);
builder.Services.RegisterIdentity();

builder.Services.RegisterRepositoryServices();
builder.Services.RegisterResourceServices();
builder.Services.RegisterDtoResourceValidators();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddEndpointsApiExplorer();
builder.Services.RegisterSwagger();

builder.Services.RegisterAuthorization();

builder.Services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.RegisterAuthentication(jwtSettings);

var app = builder.Build();

app.UseExceptionHandler("/error");
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseHsts();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BlogCore V1");
});

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<BlogCoreContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    await DbInitializer.Seed(context, roleManager, userManager);
}

app.Run();

namespace BlogCoreAPI
{
    public partial class Program { }
}
