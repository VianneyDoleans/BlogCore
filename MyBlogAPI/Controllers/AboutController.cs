using System.Collections.Generic;
using DbAccess.Data.POCO;
using Microsoft.AspNetCore.Mvc;

namespace MyBlogAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AboutController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            //TODO
            return
                Ok(new About()
                    {Version = "test"}); //typeof(Startup).Assembly.GetName().Version?.ToString() ?? "Unknown" });
            //return (Ok(new Category() {Id = 1, Name = "ok", Posts = new List<Post>()}));
        }
    }
}
