using BlogCoreAPI.Models.Settings;
using BlogCoreAPI.Services.MailService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MailService = BlogCoreAPI.Services.MailService.EmailService;


namespace BlogCoreAPI.Extensions;

public static class MailExtensions
{
    public static IServiceCollection RegisterMailService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailConfigurationSettings>(configuration.GetSection(EmailConfigurationSettings.Position));
        services.AddScoped<IEmailService, MailService>();
        return services;
    }
    
}