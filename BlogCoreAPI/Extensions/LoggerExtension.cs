using System;
using Microsoft.Extensions.Hosting;
using Serilog;
using BlogCoreAPI.Models.Exceptions;
using DBAccess.Exceptions;
using FluentValidation;
using BlogCoreAPI.Models.Logger;

namespace BlogCoreAPI.Extensions
{
    public static class LoggerExtension
    {

        public static IHostBuilder RegisterLoggerConfiguration(this IHostBuilder host)
        {
            host.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.With<UserEnricher>()
                .Filter.ByExcluding(logEvent => logEvent?.Exception != null &&
                                                (logEvent.Exception.GetType() == typeof(ResourceNotFoundException) ||
                                                 logEvent.Exception.GetType() == typeof(InvalidRequestException) ||
                                                 logEvent.Exception.GetType() == typeof(PermissionManagementException) ||
                                                 logEvent.Exception.GetType() == typeof(RoleManagementException) ||
                                                 logEvent.Exception.GetType() == typeof(UserManagementException) ||
                                                 logEvent.Exception.GetType() == typeof(ArgumentNullException) ||
                                                 logEvent.Exception.GetType() == typeof(ArgumentException) ||
                                                 logEvent.Exception.GetType() == typeof(ValidationException))));
            return host;
        }
    }
}
