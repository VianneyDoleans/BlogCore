using System;
using System.Linq;
using DbAccess.Data.POCO;
using Xunit;

namespace DBAccess.Test.Repositories
{
    public class TagsRepository
    {
        private readonly DatabaseFixture _fixture;

        public TagsRepository()
        {
            _fixture = new DatabaseFixture();
        }

        [Fact]
        public async void AddTagsAsync()
        {
            var repository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);
            var testTags = new Tag() { Name = "This is a test for tag"};
            await repository.AddAsync(testTags);
            _fixture.UnitOfWork.Save();

            Assert.True(_fixture.Db.Tags.First(x => x.Name == testTags.Name) != null);
        }

        [Fact]
        public async void AddNullTagsAsync()
        {
            var repository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.AddAsync(null));
        }

        [Fact]
        public async void GetTagsAsync()
        {
            var repository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);
            var result = await repository.GetAsync(1);

            Assert.True(result == await _fixture.Db.Tags.FindAsync(1));
        }

        [Fact]
        public async void GetTagsOutOfRangeAsync()
        {
            var repository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);

            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await repository.GetAsync(100));
        }

        [Fact]
        public async void GetAllAsync()
        {
            var repository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);
            var result = await repository.GetAllAsync();

            Assert.True(result.Count() == _fixture.Db.Tags.Count());
        }

        [Fact]
        public async void RemoveAsync()
        {
            var nbTagsAtBeginning = _fixture.Db.Tags.Count();
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);
            var testTags = new Tag() {Name = "This is a test tag"};

            await tagRepository.AddAsync(testTags);
            _fixture.UnitOfWork.Save();
            var nbTagsAfterAdded = _fixture.Db.Tags.Count();
            await tagRepository.RemoveAsync(testTags);
            _fixture.UnitOfWork.Save();
            var nbTagsAfterRemoved = _fixture.Db.Tags.Count();

            Assert.True(nbTagsAtBeginning + 1 == nbTagsAfterAdded &&
                        nbTagsAfterRemoved == nbTagsAtBeginning);
        }

        [Fact]
        public async void RemoveNullAsync()
        {
            var repository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.RemoveAsync(null));
        }
    }
}
