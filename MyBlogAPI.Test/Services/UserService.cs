using System;
using System.Collections.Generic;
using System.Text;
using DbAccess.Data.POCO;
using DBAccess.Test;
using MyBlogAPI.Services.UserService;
using Xunit;

namespace MyBlogAPI.Test.Services
{
    public class UserService : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;
        private readonly IUserService _service;

        public UserService(DatabaseFixture databaseFixture, IUserService service)
        {
            _fixture = databaseFixture;
            _service = service;
        }
    }
}
