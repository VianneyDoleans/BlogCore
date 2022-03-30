using System;
using System.Collections.Generic;
using System.Linq;
using DbAccess.Data.POCO;
using DbAccess.Specifications;
using DbAccess.Specifications.FilterSpecifications.Filters;
using DbAccess.Specifications.SortSpecification;
using DBAccess.Tests.Builders;
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
            // Arrange
            var repository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);
            var testTags = new Tag() { Name = "This is a test for tag"};

            // Act
            await repository.AddAsync(testTags);
            _fixture.UnitOfWork.Save();

            // Assert
            Assert.True(_fixture.Db.Tags.First(x => x.Name == testTags.Name) != null);
        }

        [Fact]
        public async void AddNullTagsAsync()
        {
            // Arrange
            var repository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.AddAsync(null));
        }

        [Fact]
        public async void GetTagsAsync()
        {
            // Arrange
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);
            var testTag = new TagBuilder(tagRepository, _fixture.UnitOfWork).Build();

            // Act
            var result = await tagRepository.GetAsync(testTag.Id);

            // Assert
            Assert.True(result == await _fixture.Db.Tags.FindAsync(testTag.Id));
        }

        [Fact]
        public async void GetTagsOutOfRangeAsync()
        {
            // Arrange
            var repository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);

            // Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await repository.GetAsync(100));
        }

        [Fact]
        public async void GetAllAsync()
        {
            // Arrange
            var repository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);
            
            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.True(result.Count() == _fixture.Db.Tags.Count());
        }

        [Fact]
        public async void RemoveAsync()
        {
            // Arrange
            var nbTagsAtBeginning = _fixture.Db.Tags.Count();
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);
            var testTags = new TagBuilder(tagRepository, _fixture.UnitOfWork).Build();
            var nbTagsAfterAdded = _fixture.Db.Tags.Count();

            // Act
            await tagRepository.RemoveAsync(testTags);
            _fixture.UnitOfWork.Save();
            
            // Assert
            var nbTagsAfterRemoved = _fixture.Db.Tags.Count();
            Assert.True(nbTagsAtBeginning + 1 == nbTagsAfterAdded &&
                        nbTagsAfterRemoved == nbTagsAtBeginning);
        }

        [Fact]
        public async void RemoveNullAsync()
        {
            // Arrange
            var repository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);

            // Act & Assert
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
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncSpecification").Build();
            var testTag = new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncSpecification2").Build();
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncSpecification3").Build();
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncSpecification4").Build();

            // Act
            var result = await tagRepository.GetAsync(new IdSpecification<Tag>(testTag.Id));

            // Assert
            Assert.True(result.First().Id == testTag.Id);
        }

        [Fact]
        public async void GetAsyncWithTwoSpecifications()
        {
            // Arrange
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("GetAWiTwoSpecTag").Build();
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("GetAWiTwoSpecAKTag2").Build();
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("AKGetAWiTwoSpecTag3").Build();
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("GetAWiTwoSpecPst4").Build();
            var testTag = new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("GetAWiTwoSpecTagAK5164").Build();
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("GetAWiTwoSpecTag6").Build();

            // Act
            var result = (await tagRepository.GetAsync(new NameContainsSpecification<Tag>("AK") & new NameContainsSpecification<Tag>("164"))).ToList();

            // Assert
            Assert.True(result.Count() == 1 && result.First().Name == testTag.Name);
        }

        [Fact]
        public async void GetAsyncWithTwoSortsAndTwoSpecificationsAndPagination()
        {
            // Arrange
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TwwooGetAsyncWithTwoSortsTag").Build();
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("GetAsyncWithTwoSorts2Tag").Build();
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("GetAsyncWithTwoSorts3TwwooTag").Build();
            var tag = new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("AGetTwwooAsyncWithTwoSorts4Tag").Build();
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("GetAsyncTwwooWithTwoSorts5Tag").Build();
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("GetAsyncWithTwoSorts6Tag").Build();
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TGetAsyncWithTwoorts7Tag").Build();

            // Act
            var result = (await tagRepository.GetAsync(new NameContainsSpecification<Tag>("WithTwoSorts")
                                                            & new NameContainsSpecification<Tag>("Twwoo"), new PagingSpecification(0, 20),
                new SortSpecification<Tag>(new OrderBySpecification<Tag>(x => x.PostTags.Count), SortingDirectionSpecification.Descending) &
                new SortSpecification<Tag>(new OrderBySpecification<Tag>(x => x.Name), SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 4 && result.First().Name == tag.Name);
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
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncWithAllArguments").Build();
            var testTag = new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncAKWithAllArguments2").Build();
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncAkWithAllArguments3").Build();
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncWithAllArguments4").Build();
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagAKGetAsyncWithAllArguments5").Build();
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncWithAllArguments6").Build();

            // Act
            var result = (await tagRepository.GetAsync(new NameContainsSpecification<Tag>("AK") &
                new NameContainsSpecification<Tag>("WithAllArguments"),
                new PagingSpecification(0, 2),
                new SortSpecification<Tag>(
                    new OrderBySpecification<Tag>(x => x.Name),
                    SortingDirectionSpecification.Descending))).ToList();

            // Assert
            Assert.True(result.Count() == 2 && result.First().Name == testTag.Name);
        }

        [Fact]
        public async void GetAsyncWithPagination()
        {
            // Arrange
            var tagRepository = new DbAccess.Repositories.Tag.TagRepository(_fixture.Db);
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("Tag1GetAsyncWithPagination1").Build();
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("Tag1GetAsyncWithPagination2").Build();
            var testTag3 = new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("Tag1GetAsyncWithPagination3").Build();
            var testTag4 = new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("Tag1GetAsyncWithPagination4").Build();
            var testTag5 = new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("Tag1GetAsyncWithPagination5").Build();
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("Tag1GetAsyncWithPagination6").Build();

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
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncWithPaginationTakeOutOfRange1").Build();
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncWithPaginationTakeOutOfRange2").Build();
            var testTag3 = new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncWithPaginationTakeOutOfRange3").Build();
            var testTag5 = new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncWithPaginationTakeOutOfRange4").Build();
            var testTag6 = new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncWithPaginationTakeOutOfRange5").Build();
            var testTag4 = new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncWithPaginationTakeOutOfRange6").Build();

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
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncWithPaginationTakeNegative1").Build();
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncWithPaginationTakeNegative2").Build();
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncWithPaginationTakeNegative3").Build(); 
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncWithPaginationTakeNegative4").Build();
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncWithPaginationTakeNegative5").Build();
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncWithPaginationTakeNegative6").Build();

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
            var testTag = new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncWithPaginationSkipNegative").Build();
            var testTag2 = new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncWithPaginationSkipNegative2").Build();
            var testTag3 = new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncWithPaginationSkipNegative3").Build();
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncWithPaginationSkipNegative4").Build();
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncWithPaginationSkipNegative5").Build();
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncWithPaginationSkipNegative6").Build();

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
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncWithPaginationSkipOutOfRange").Build();
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncWithPaginationSkipOutOfRange2").Build();
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncWithPaginationSkipOutOfRange3").Build();
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncWithPaginationSkipOutOfRange4").Build();
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncWithPaginationSkipOutOfRange5").Build();
            new TagBuilder(tagRepository, _fixture.UnitOfWork).WithName("TagGetAsyncWithPaginationSkipOutOfRange6").Build();

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
            var testTag = new TagBuilder(tagRepository, _fixture.UnitOfWork).Build();
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
            var testTag = new TagBuilder(tagRepository, _fixture.UnitOfWork).Build();

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
            var testTag = new TagBuilder(tagRepository, _fixture.UnitOfWork).Build();
            var testTag2 = new TagBuilder(tagRepository, _fixture.UnitOfWork).Build();
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
            var testTag = new TagBuilder(tagRepository, _fixture.UnitOfWork).Build();
            var testTag2 = new TagBuilder(tagRepository, _fixture.UnitOfWork).Build();
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
            var testTag = new TagBuilder(tagRepository, _fixture.UnitOfWork).Build();

            // Act & Assert
            Assert.True(await tagRepository.NameAlreadyExists(testTag.Name));
        }
    }
}
