using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using MyBlogAPI.DTO.User;
using MyBlogAPI.IntegrationTests.GenericTests;
using MyBlogAPI.IntegrationTests.Helpers;
using Newtonsoft.Json;
using Xunit;

namespace MyBlogAPI.IntegrationTests.Controllers
{
    public sealed class UsersController : AGenericTests<GetUserDto, AddUserDto, UpdateUserDto>, IClassFixture<TestWebApplicationFactory>
    {
        protected override IEntityHelper<GetUserDto, AddUserDto, UpdateUserDto> Helper { get; set; }

        public UsersController(TestWebApplicationFactory factory) : base(factory)
        {
            Helper = new UserHelper(Client);
        }
    }
}
