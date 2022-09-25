using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BlogCoreAPI;
using BlogCoreAPI.Extensions;
using BlogCoreAPI.Extensions.FluentValidation;
using DBAccess;
using DBAccess.Data;
using DBAccess.DataContext;
using DBAccess.Extensions;
using DBAccess.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Host.RegisterLoggerConfiguration();
builder.Services.AddHttpContextAccessor();
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

builder.Services.RegisterJwt(configuration);
var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();
builder.Services.RegisterAuthentication(jwtSettings);

ConfigureHttpLogging();

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

app.UseHttpLogging()
    .UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await FillInDatabase();

app.Run();

void ConfigureHttpLogging()
{
    builder.Services.AddHttpLogging(logging =>
    {
        logging.LoggingFields = HttpLoggingFields.All;
        logging.RequestHeaders.Add(HeaderNames.Accept);
        logging.RequestHeaders.Add(HeaderNames.ContentType);
        logging.RequestHeaders.Add(HeaderNames.ContentDisposition);
        logging.RequestHeaders.Add(HeaderNames.ContentEncoding);
        logging.RequestHeaders.Add(HeaderNames.ContentLength);

        logging.MediaTypeOptions.AddText("application/json");
        logging.MediaTypeOptions.AddText("multipart/form-data");

        logging.RequestBodyLogLimit = 4096;
        logging.ResponseBodyLogLimit = 4096;
    });
}

async Task FillInDatabase()
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<BlogCoreContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    await DbInitializer.Seed(context, roleManager, userManager);
}

namespace BlogCoreAPI
{
    public partial class Program { }
}
