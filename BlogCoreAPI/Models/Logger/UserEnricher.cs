using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace BlogCoreAPI.Models.Logger
{
    public class UserEnricher : ILogEventEnricher
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserEnricher() : this(new HttpContextAccessor())
        {
        }

        //Dependency injection can be used to retrieve any service required to get a user or any data.
        //Here, I easily get data from HTTPContext
        public UserEnricher(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var userName = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name) ?? "anonymous";
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "N/A";
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                "Username", userName));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                "UserId", userId));
        }
    }
}
