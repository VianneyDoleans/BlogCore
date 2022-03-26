using System;
using System.Collections.Generic;
using System.Linq;
using DbAccess.Data.POCO;
using DbAccess.Specifications;
using DbAccess.Specifications.FilterSpecifications.Filters;
using DbAccess.Specifications.SortSpecification;
using Xunit;

namespace DBAccess.Tests.Repositories
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
            var testTag = new Tag()
            {
                Name = "GetTagsAsync"
            };
            await repository.AddAsync(testTag);
            _fixture.UnitOfWork.Save();

            var result = await repository.GetAsync(testTag.Id);

            Assert.True(result == await _fixture.Db.Tags.FindAsync(testTag.Id));
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

        // --------

        [Fact]
        public void AddTag()
        {
            // Arrange
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);
            _fixture.Db.SaveChanges();
            var testTag = new Tag() { Name = "AddTag" };

            // Act
            tagRepository.Add(testTag);
            _fixture.UnitOfWork.Save();

            // Assert
            Assert.True(_fixture.Db.Tags.First(x => x.Name == "AddTag") != null);
        }

        [Fact]
        public void AddNullTag()
        {
            // Arrange
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                tagRepository.Add(null);
                _fixture.Db.SaveChanges();
            });
        }

        [Fact]
        public void CountAll()
        {
            // Arrange
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);

            // Act & Assert
            Assert.True(_fixture.Db.Tags.Count() == tagRepository.CountAll());
        }

        [Fact]
        public async void CountAllAsync()
        {
            // Arrange
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);

            // Act & Assert
            Assert.True(_fixture.Db.Tags.Count() == await tagRepository.CountAllAsync());
        }


        [Fact]
        public void GetAll()
        {
            // Arrange
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);

            // Act
            var result = tagRepository.GetAll();

            // Assert
            Assert.True(result.Count() == _fixture.Db.Tags.Count());
        }

        [Fact]
        public async void GetAsyncSpecificationBasic()
        {
            // Arrange
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);
            _fixture.Db.SaveChanges();
            var testTag = new Tag()
            {
                Name = "TagGetAsyncSpecification"
            };
            var testTag2 = new Tag()
            {
                Name = "TagGetAsyncSpecification2"
            };
            var testTag3 = new Tag()
            {
                Name = "TagGetAsyncSpecification3"
            };
            var testTag4 = new Tag()
            {
                Name = "TagGetAsyncSpecification4"
            };
            tagRepository.Add(testTag);
            tagRepository.Add(testTag2);
            tagRepository.Add(testTag3);
            tagRepository.Add(testTag4);
            _fixture.UnitOfWork.Save();

            // Act
            var result = await tagRepository.GetAsync(new IdSpecification<Tag>(testTag2.Id));

            // Assert
            Assert.True(result.First().Id == testTag2.Id);
        }

        [Fact]
        public async void GetAsyncWithTwoSpecifications()
        {
            // Arrange
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);
            _fixture.Db.SaveChanges();
            var testTag = new Tag()
            {
                Name = "GetAWiTwoSpecTag"
            };
            var testTag2 = new Tag()
            {
                Name = "GetAWiTwoSpecAKTag2"
            };
            var testTag3 = new Tag()
            {
                Name = "AKGetAWiTwoSpecTag3"
            };
            var testTag4 = new Tag()
            {
                Name = "GetAWiTwoSpecPst4"
            };
            var testTag5 = new Tag()
            {
                Name = "GetAWiTwoSpecTagAK5164"
            };
            var testTag6 = new Tag()
            {
                Name = "GetAWiTwoSpecTag6"
            };
            await tagRepository.AddAsync(testTag);
            await tagRepository.AddAsync(testTag2);
            await tagRepository.AddAsync(testTag3);
            await tagRepository.AddAsync(testTag4);
            await tagRepository.AddAsync(testTag5);
            await tagRepository.AddAsync(testTag6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await tagRepository.GetAsync(new NameContainsSpecification<Tag>("AK") & new NameContainsSpecification<Tag>("164"))).ToList();

            // Assert
            Assert.True(result.Count() == 1 && result.First().Name == testTag5.Name);
        }

        [Fact]
        public async void GetAsyncWithTwoSortsAndTwoSpecificationsAndPagination()
        {
            // Arrange
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);
            await _fixture.Db.Tags.AddAsync(
                new Tag() { Name = "TwwooGetAsyncWithTwoSortsTag" });
            await _fixture.Db.Tags.AddAsync(
                new Tag() { Name = "GetAsyncWithTwoSorts2Tag" });
            await _fixture.Db.Tags.AddAsync(
                new Tag() { Name = "GetAsyncWithTwoSorts3TwwooTag" });
            var tag4 = await _fixture.Db.Tags.AddAsync(
                new Tag() { Name = "AGetTwwooAsyncWithTwoSorts4Tag" });
            await _fixture.Db.Tags.AddAsync(
                new Tag() { Name = "GetAsyncTwwooWithTwoSorts5Tag" });
            await _fixture.Db.Tags.AddAsync(
                new Tag() { Name = "GetAsyncWithTwoSorts6Tag" });
            await _fixture.Db.Tags.AddAsync(
                new Tag() { Name = "TGetAsyncWithTwoorts7Tag" });
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await tagRepository.GetAsync(new NameContainsSpecification<Tag>("WithTwoSorts")
                                                            & new NameContainsSpecification<Tag>("Twwoo"), new PagingSpecification(0, 20),
                new SortSpecification<Tag>(new OrderBySpecification<Tag>(x => x.PostTags.Count), SortingDirectionSpecification.Descending) &
                new SortSpecification<Tag>(new OrderBySpecification<Tag>(x => x.Name), SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 4 && result.First().Name == tag4.Entity.Name);
        }

        [Fact]
        public async void GetAsyncWithNoArgument()
        {
            // Arrange
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);

            // Act & Assert
            Assert.True((await tagRepository.GetAsync()).ToList().Count() == _fixture.Db.Tags.Count());
        }

        [Fact]
        public async void GetAsyncWithAllArguments()
        {
            // Arrange
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);
            var testTag = new Tag()
            {
                Name = "TagGetAsyncWithAllArguments"
            };
            var testTag2 = new Tag()
            {
                Name = "TagGetAsyncAKWithAllArguments2"
            };
            var testTag3 = new Tag()
            {
                Name = "TagGetAsyncAkWithAllArguments3"
            };
            var testTag4 = new Tag()
            {
                Name = "TagGetAsyncWithAllArguments4"
            };
            var testTag5 = new Tag()
            {
                Name = "TagAKGetAsyncWithAllArguments5"
            };
            var testTag6 = new Tag()
            {
                Name = "TagGetAsyncWithAllArguments6"
            };
            await tagRepository.AddAsync(testTag);
            await tagRepository.AddAsync(testTag2);
            await tagRepository.AddAsync(testTag3);
            await tagRepository.AddAsync(testTag4);
            await tagRepository.AddAsync(testTag5);
            await tagRepository.AddAsync(testTag6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await tagRepository.GetAsync(new NameContainsSpecification<Tag>("AK") &
                new NameContainsSpecification<Tag>("WithAllArguments"),
                new PagingSpecification(0, 2),
                new SortSpecification<Tag>(
                    new OrderBySpecification<Tag>(x => x.Name),
                    SortingDirectionSpecification.Descending))).ToList();

            // Assert
            Assert.True(result.Count() == 2 && result.First().Name == testTag2.Name);
        }

        [Fact]
        public async void GetAsyncWithPagination()
        {
            // Arrange
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);
            await _fixture.Db.Tags.AddAsync(
                new Tag() { Name = "GetAsyncWithPagination" });
            var testTag = new Tag()
            {
                Name = "Tag1GetAsyncWithPagination1"
            };
            var testTag2 = new Tag()
            {
                Name = "Tag1GetAsyncWithPagination2"
            };
            var testTag3 = new Tag()
            {
                Name = "Tag1GetAsyncWithPagination3"
            };
            var testTag4 = new Tag()
            {
                Name = "Tag1GetAsyncWithPagination4"
            };
            var testTag5 = new Tag()
            {
                Name = "Tag1GetAsyncWithPagination5"
            };
            var testTag6 = new Tag()
            {
                Name = "Tag1GetAsyncWithPagination6"
            };
            await tagRepository.AddAsync(testTag);
            await tagRepository.AddAsync(testTag2);
            await tagRepository.AddAsync(testTag3);
            await tagRepository.AddAsync(testTag4);
            await tagRepository.AddAsync(testTag5);
            await tagRepository.AddAsync(testTag6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await tagRepository.GetAsync(new NameContainsSpecification<Tag>("1GetAsyncWithPagination"),
                new PagingSpecification(2, 3),
                new SortSpecification<Tag>(
                    new OrderBySpecification<Tag>(x => x.Name),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 3);
            Assert.Contains(result, x => x.Id == testTag3.Id &&
                                         x.Name == testTag3.Name);
            Assert.Contains(result, x => x.Id == testTag4.Id &&
                                         x.Name == testTag4.Name);
            Assert.Contains(result, x => x.Id == testTag5.Id &&
                                         x.Name == testTag5.Name);
        }

        [Fact]
        public async void GetAsyncWithPaginationTakeOutOfRange()
        {
            // Arrange
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);
            var testTag = new Tag()
            {
                Name = "TagGetAsyncWithPaginationTakeOutOfRange1"
            };
            var testTag2 = new Tag()
            {
                Name = "TagGetAsyncWithPaginationTakeOutOfRange2"
            };
            var testTag3 = new Tag()
            {
                Name = "TagGetAsyncWithPaginationTakeOutOfRange3"
            };
            var testTag5 = new Tag()
            {
                Name = "TagGetAsyncWithPaginationTakeOutOfRange4"
            };
            var testTag6 = new Tag()
            {
                Name = "TagGetAsyncWithPaginationTakeOutOfRange5"
            };
            var testTag4 = new Tag()
            {
                Name = "TagGetAsyncWithPaginationTakeOutOfRange6"
            };
            await tagRepository.AddAsync(testTag);
            await tagRepository.AddAsync(testTag2);
            await tagRepository.AddAsync(testTag3);
            await tagRepository.AddAsync(testTag4);
            await tagRepository.AddAsync(testTag5);
            await tagRepository.AddAsync(testTag6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await tagRepository.GetAsync(new NameContainsSpecification<Tag>("GetAsyncWithPaginationTakeOutOfRange"),
                new PagingSpecification(2, 22),
                new SortSpecification<Tag>(
                    new OrderBySpecification<Tag>(x => x.Name),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 4);
            Assert.Contains(result, x => x.Id == testTag3.Id &&
                                         x.Name == testTag3.Name);
            Assert.Contains(result, x => x.Id == testTag4.Id &&
                                         x.Name == testTag4.Name);
            Assert.Contains(result, x => x.Id == testTag5.Id &&
                                         x.Name == testTag5.Name);
            Assert.Contains(result, x => x.Id == testTag6.Id &&
                                         x.Name == testTag6.Name);
        }

        [Fact]
        public async void GetAsyncWithPaginationTakeNegative()
        {
            // Arrange
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);
            var testTag = new Tag()
            {
                Name = "TagGetAsyncWithPaginationTakeNegative1"
            };
            var testTag2 = new Tag()
            {
                Name = "TagGetAsyncWithPaginationTakeNegative2"
            };
            var testTag3 = new Tag()
            {
                Name = "TagGetAsyncWithPaginationTakeNegative3"
            };
            var testTag5 = new Tag()
            {
                Name = "TagGetAsyncWithPaginationTakeNegative4"
            };
            var testTag6 = new Tag()
            {
                Name = "TagGetAsyncWithPaginationTakeNegative5"
            };
            var testTag4 = new Tag()
            {
                Name = "TagGetAsyncWithPaginationTakeNegative6"
            };
            await tagRepository.AddAsync(testTag);
            await tagRepository.AddAsync(testTag2);
            await tagRepository.AddAsync(testTag3);
            await tagRepository.AddAsync(testTag4);
            await tagRepository.AddAsync(testTag5);
            await tagRepository.AddAsync(testTag6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await tagRepository.GetAsync(new NameContainsSpecification<Tag>("GetAsyncWithPaginationTakeNegative"),
                new PagingSpecification(2, -2),
                new SortSpecification<Tag>(
                    new OrderBySpecification<Tag>(x => x.Name),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(!result.Any());
        }

        [Fact]
        public async void GetAsyncWithPaginationSkipNegative()
        {
            // Arrange
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);
            await _fixture.Db.Tags.AddAsync(
                new Tag() { Name = "ComGetAWithPagSkipNega" });
            var testTag = new Tag()
            {
                Name = "TagGetAsyncWithPaginationSkipNegative"
            };
            var testTag2 = new Tag()
            {
                Name = "TagGetAsyncWithPaginationSkipNegative2"
            };
            var testTag3 = new Tag()
            {
                Name = "TagGetAsyncWithPaginationSkipNegative3"
            };
            var testTag4 = new Tag()
            {
                Name = "TagGetAsyncWithPaginationSkipNegative4"
            };
            var testTag5 = new Tag()
            {
                Name = "TagGetAsyncWithPaginationSkipSkipNegative5"
            };
            var testTag6 = new Tag()
            {
                Name = "TagGetAsyncWithPaginationSkipSkipNegative6"
            };
            await tagRepository.AddAsync(testTag);
            await tagRepository.AddAsync(testTag2);
            await tagRepository.AddAsync(testTag3);
            await tagRepository.AddAsync(testTag4);
            await tagRepository.AddAsync(testTag5);
            await tagRepository.AddAsync(testTag6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await tagRepository.GetAsync(new NameContainsSpecification<Tag>("GetAsyncWithPaginationSkipNegative"),
                new PagingSpecification(-2, 3),
                new SortSpecification<Tag>(
                    new OrderBySpecification<Tag>(x => x.Name),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 3);
            Assert.Contains(result, x => x.Id == testTag.Id &&
                                         x.Name == testTag.Name);
            Assert.Contains(result, x => x.Id == testTag2.Id &&
                                         x.Name == testTag2.Name);
            Assert.Contains(result, x => x.Id == testTag3.Id &&
                                         x.Name == testTag3.Name);
        }

        [Fact]
        public async void GetAsyncWithPaginationSkipOutOfRange()
        {
            // Arrange
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);
            var testTag = new Tag()
            {
                Name = "TagGetAsyncWithPaginationSkipOutOfRange"
            };
            var testTag2 = new Tag()
            {
                Name = "TagGetAsyncWithPaginationSkipOutOfRange2"
            };
            var testTag3 = new Tag()
            {
                Name = "TagGetAsyncWithPaginationSkipOutOfRange3"
            };
            var testTag5 = new Tag()
            {
                Name = "TagGetAsyncWithPaginationSkipOutOfRange4"
            };
            var testTag6 = new Tag()
            {
                Name = "TagGetAsyncWithPaginationSkipOutOfRange5"
            };
            var testTag4 = new Tag()
            {
                Name = "TagGetAsyncWithPaginationSkipOutOfRange6"
            };
            await tagRepository.AddAsync(testTag);
            await tagRepository.AddAsync(testTag2);
            await tagRepository.AddAsync(testTag3);
            await tagRepository.AddAsync(testTag4);
            await tagRepository.AddAsync(testTag5);
            await tagRepository.AddAsync(testTag6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await tagRepository.GetAsync(new NameContainsSpecification<Tag>("GetAsyncWithPaginationSkipOutOfRange"),
                new PagingSpecification(7, 3),
                new SortSpecification<Tag>(
                    new OrderBySpecification<Tag>(x => x.Name),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(!result.Any());
        }

        [Fact]
        public void Remove()
        {
            // Arrange
            var nbCategoriesAtBeginning = _fixture.Db.Tags.Count();
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);
            var testTag = new Tag()
            {
                Name = "RemoveTag"
            };
            tagRepository.Add(testTag);
            _fixture.UnitOfWork.Save();
            var nbCategoriesAfterAdded = _fixture.Db.Tags.Count();

            // Act
            tagRepository.Remove(testTag);
            _fixture.UnitOfWork.Save();

            // Assert
            var nbCategoriesAfterRemoved = _fixture.Db.Tags.Count();
            Assert.True(nbCategoriesAtBeginning + 1 == nbCategoriesAfterAdded &&
                        nbCategoriesAfterRemoved == nbCategoriesAtBeginning);
        }

        [Fact]
        public void GetTag()
        {
            // Arrange
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);
            var testTag = new Tag()
            {
                Name = "GetTag"
            };
            tagRepository.Add(testTag);
            _fixture.UnitOfWork.Save();

            // Act
            var result = tagRepository.Get(testTag.Id);

            // Act & Assert
            Assert.True(result == _fixture.Db.Tags.Find(testTag.Id));
        }

        [Fact]
        public void GetCategoryOutOfRange()
        {
            // Arrange
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<IndexOutOfRangeException>(() => tagRepository.Get(100));
        }

        [Fact]
        public void RemoveNull()
        {
            // Arrange
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => tagRepository.Remove(null));
        }

        [Fact]
        public async void RemoveRangeAsyncNull()
        {
            // Arrange
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await tagRepository.RemoveRangeAsync(null));
        }

        [Fact]
        public void RemoveRangeNull()
        {
            // Arrange
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => tagRepository.RemoveRange(null));
        }

        [Fact]
        public void RemoveRange()
        {
            // Arrange
            var nbCategoriesAtBeginning = _fixture.Db.Tags.Count();
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);
            var testTag = new Tag()
            {
                Name = "TagRemoveRange1"
            };
            var testTag2 = new Tag()
            {
                Name = "TagRemoveRange2"
            };
            tagRepository.Add(testTag);
            tagRepository.Add(testTag2);
            _fixture.UnitOfWork.Save();
            var nbCategoriesAfterAdded = _fixture.Db.Tags.Count();

            // Act
            tagRepository.RemoveRange(new List<Tag>() { testTag, testTag2 });
            _fixture.UnitOfWork.Save();

            // Assert
            var nbCategoriesAfterRemoved = _fixture.Db.Tags.Count();
            Assert.True(nbCategoriesAtBeginning + 2 == nbCategoriesAfterAdded &&
                        nbCategoriesAfterRemoved == nbCategoriesAtBeginning);
        }

        [Fact]
        public async void RemoveRangeAsync()
        {
            // Arrange
            var nbCategoriesAtBeginning = _fixture.Db.Tags.Count();
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);
            var testTag = new Tag()
            {
                Name = "TagRemoveRangeAsync1"
            };
            var testTag2 = new Tag()
            {
                Name = "TagRemoveRangeAsync2"
            };
            await tagRepository.AddAsync(testTag);
            await tagRepository.AddAsync(testTag2);
            _fixture.UnitOfWork.Save();
            var nbCategoriesAfterAdded = _fixture.Db.Tags.Count();

            // Act
            await tagRepository.RemoveRangeAsync(new List<Tag>() { testTag, testTag2 });
            _fixture.UnitOfWork.Save();

            // Assert
            var nbCategoriesAfterRemoved = _fixture.Db.Tags.Count();
            Assert.True(nbCategoriesAtBeginning + 2 == nbCategoriesAfterAdded &&
                        nbCategoriesAfterRemoved == nbCategoriesAtBeginning);
        }

        [Fact]
        public async void NameAlreadyExistsFalse()
        {
            // Arrange
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);

            // Act & Assert
            Assert.True(!await tagRepository.NameAlreadyExists("NameAlreadyExistsFalse"));
        }

        [Fact]
        public async void NameAlreadyExistsNull()
        {
            // Arrange
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);

            // Act & Assert
            Assert.True(!await tagRepository.NameAlreadyExists(null));
        }

        [Fact]
        public async void NameAlreadyExistsTrue()
        {
            // Arrange
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);
            var testTag = new Tag()
            {
                Name = "NameAlreadyExistsTrue",
            };
            await tagRepository.AddAsync(testTag);
            await _fixture.Db.SaveChangesAsync();

            // Act & Assert
            Assert.True(await tagRepository.NameAlreadyExists(testTag.Name));
        }
    }
}
