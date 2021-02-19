using System;
using System.Linq;
using DbAccess.Data.POCO;
using Xunit;

namespace DBAccess.Test.Repositories
{
    public class RolesRepository : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public RolesRepository(DatabaseFixture databaseFixture)
        {
            _fixture = databaseFixture;
        }

        [Fact]
        public async void AddRolesAsync()
        {
            var repository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);
            var testRoles = new Role() { Name = "This is a test for role" };
            await repository.AddAsync(testRoles);
            _fixture.UnitOfWork.Save();

            Assert.True(_fixture.Db.Roles.First(x => x.Name == testRoles.Name) != null);
        }

        [Fact]
        public async void AddNullRolesAsync()
        {
            var repository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.AddAsync(null));
        }

        [Fact]
        public async void GetRolesAsync()
        {
            var repository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);
            var result = await repository.GetAsync(1);

            Assert.True(result == await _fixture.Db.Roles.FindAsync(1));
        }

        [Fact]
        public async void GetRolesOutOfRangeAsync()
        {
            var repository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await repository.GetAsync(100));
        }

        [Fact]
        public async void GetAllAsync()
        {
            var repository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);
            var result = await repository.GetAllAsync();

            Assert.True(result.Count() == _fixture.Db.Roles.Count());
        }

        [Fact]
        public async void RemoveAsync()
        {
            var nbRolesAtBeginning = _fixture.Db.Roles.Count();
            var tagRepository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);
            var testRoles = new Role() { Name = "This is a test role" };

            await tagRepository.AddAsync(testRoles);
            _fixture.UnitOfWork.Save();
            var nbRolesAfterAdded = _fixture.Db.Roles.Count();
            await tagRepository.RemoveAsync(testRoles);
            _fixture.UnitOfWork.Save();
            var nbRolesAfterRemoved = _fixture.Db.Roles.Count();

            Assert.True(nbRolesAtBeginning + 1 == nbRolesAfterAdded &&
                        nbRolesAfterRemoved == nbRolesAtBeginning);
        }

        [Fact]
        public async void RemoveNullAsync()
        {
            var repository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.RemoveAsync(null));
        }
    }
}
