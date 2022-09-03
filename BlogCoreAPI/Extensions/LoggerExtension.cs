using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace BlogCoreAPI.Extensions
{
    public static class LoggerExtension
    {

        public static IHostBuilder RegisterLogger(this IHostBuilder host, IConfiguration configuration)
        {
            host.UseSerilog((ctx, lc) => lc
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext());
            return host;
        }
    }
}
