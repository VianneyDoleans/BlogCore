using System;
using System.Linq;
using DbAccess.Data.POCO;
using Xunit;

namespace DBAccess.Test.Repositories
{
    public class UserRepository : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public UserRepository(DatabaseFixture databaseFixture)
        {
            _fixture = databaseFixture;
        }

        [Fact]
        public async void AddUserAsync()
        {
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);
            var testUser = new User()
            {
                EmailAddress = "test@test.com",
                Username = "test",
                Password = "testPassword"
            };
            await userRepository.AddAsync(testUser);
            _fixture.UnitOfWork.Save();

            Assert.True(_fixture.Db.Users.First(x => x.EmailAddress == testUser.EmailAddress &&
                                                     x.Username == testUser.Username &&
                                                     x.Password == testUser.Password) != null);
        }

        [Fact]
        public async void AddNullUserAsync()
        {
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await userRepository.AddAsync(null));
        }

        [Fact]
        public async void GetUserAsync()
        {
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);
            var result = await userRepository.GetAsync(1);
            
            Assert.True(result == await _fixture.Db.Users.FindAsync(1));
        }

        [Fact]
        public async void GetUserOutOfRangeAsync()
        {
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await userRepository.GetAsync(100));
        }

        [Fact]
        public async void GetAllAsync()
        {
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);
            var result = await userRepository.GetAllAsync();

            Assert.True(result.Count() == _fixture.Db.Users.Count());
        }

        [Fact]
        public async void RemoveAsync()
        {
            var nbUsersAtBeginning = _fixture.Db.Users.Count();
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);
            var testUser = new User()
            {
                EmailAddress = "test@test.com",
                Username = "test",
                Password = "testPassword"
            };

            await userRepository.AddAsync(testUser);
            _fixture.UnitOfWork.Save();
            var nbUserAfterAdded = _fixture.Db.Users.Count();
            await userRepository.RemoveAsync(testUser);
            _fixture.UnitOfWork.Save();
            var nbUserAfterRemoved = _fixture.Db.Users.Count();

            Assert.True(nbUsersAtBeginning + 1 == nbUserAfterAdded &&
                        nbUserAfterRemoved == nbUsersAtBeginning);
        }

        [Fact]
        public async void RemoveNullAsync()
        {
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await userRepository.RemoveAsync(null));
        }

    }
}
