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
    public class UserRepository
    {
        private readonly DatabaseFixture _fixture;

        public UserRepository()
        {
            _fixture = new DatabaseFixture();
        }

        [Fact]
        public async void AddUserAsync()
        {
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);
            var testUser = new User()
            {
                EmailAddress = "test@test.com",
                UserName = "test",
                Password = "testPassword"
            };
            await userRepository.AddAsync(testUser);
            _fixture.UnitOfWork.Save();

            Assert.True(_fixture.Db.Users.First(x => x.EmailAddress == testUser.EmailAddress &&
                                                     x.UserName == testUser.UserName &&
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

            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await userRepository.GetAsync(100));
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
                UserName = "test",
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

        // --------

        [Fact]
        public void AddUser()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);
            _fixture.Db.SaveChanges();
            var testUser = new User() { EmailAddress = "", Password = "test", UserName = "AddUser" };

            // Act
            userRepository.Add(testUser);
            _fixture.UnitOfWork.Save();

            // Assert
            Assert.True(_fixture.Db.Users.First(x => x.UserName == "AddUser") != null);
        }

        [Fact]
        public void AddNullUser()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                userRepository.Add(null);
                _fixture.Db.SaveChanges();
            });
        }

        [Fact]
        public void CountAll()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);

            // Act & Assert
            Assert.True(_fixture.Db.Users.Count() == userRepository.CountAll());
        }

        [Fact]
        public async void CountAllAsync()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);

            // Act & Assert
            Assert.True(_fixture.Db.Users.Count() == await userRepository.CountAllAsync());
        }


        [Fact]
        public void GetAll()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);

            // Act
            var result = userRepository.GetAll();

            // Assert
            Assert.True(result.Count() == _fixture.Db.Users.Count());
        }

        [Fact]
        public async void GetAsyncSpecificationBasic()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);
            _fixture.Db.SaveChanges();
            var testUser = new User()
            {
                EmailAddress = "UserGetAsyncSpecification@test.test", Password = "test", UserName = "UserGetAsyncSpecification"
            };
            var testUser2 = new User()
            {
                EmailAddress = "UserGetAsyncSpecification2@test.test", Password = "test", UserName = "UserGetAsyncSpecification2"
            };
            var testUser3 = new User()
            {
                EmailAddress = "UserGetAsyncSpecification3@test.test", Password = "test", UserName = "UserGetAsyncSpecification3"
            };
            var testUser4 = new User()
            {
                EmailAddress = "UserGetAsyncSpecification4@test.test", Password = "test", UserName = "UserGetAsyncSpecification4"
            };
            userRepository.Add(testUser);
            userRepository.Add(testUser2);
            userRepository.Add(testUser3);
            userRepository.Add(testUser4);
            _fixture.UnitOfWork.Save();

            // Act
            var result = await userRepository.GetAsync(new IdSpecification<User>(testUser2.Id));

            // Assert
            Assert.True(result.First().Id == testUser2.Id);
        }

        [Fact]
        public async void GetAsyncWithTwoSpecifications()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);
            _fixture.Db.SaveChanges();
            var testUser = new User()
            {
                EmailAddress = "GetAWiTwoSpecUser@test.test", Password = "test", UserName = "GetAWiTwoSpecUser"
            };
            var testUser2 = new User()
            {
                EmailAddress = "GetAWiTwoSpecUser2@test.test", Password = "test", UserName = "GetAWiTwoSpecAKUser2"
            };
            var testUser3 = new User()
            {
                EmailAddress = "GetAWiTwoSpecUser3@test.test", Password = "test", UserName = "AKGetAWiTwoSpecUser3"
            };
            var testUser4 = new User()
            {
                EmailAddress = "GetAWiTwoSpecUser4@test.test", Password = "test", UserName = "GetAWiTwoSpecPst4"
            };
            var testUser5 = new User()
            {
                EmailAddress = "GetAWiTwoSpecUser5@test.test", Password = "test", UserName = "GetAWiTwoSpecUserAK5164"
            };
            var testUser6 = new User()
            {
                EmailAddress = "GetAWiTwoSpecUser6@test.test", Password = "test", UserName = "GetAWiTwoSpecUser6"
            };
            await userRepository.AddAsync(testUser);
            await userRepository.AddAsync(testUser2);
            await userRepository.AddAsync(testUser3);
            await userRepository.AddAsync(testUser4);
            await userRepository.AddAsync(testUser5);
            await userRepository.AddAsync(testUser6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await userRepository.GetAsync(new UsernameContainsSpecification<User>("AK") & new UsernameContainsSpecification<User>("164"))).ToList();

            // Assert
            Assert.True(result.Count() == 1 && result.First().UserName == testUser5.UserName);
        }

        [Fact]
        public async void GetAsyncWithTwoSortsAndTwoSpecificationsAndPagination()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);
            await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "TwwooGetAsyncWithTwoSortsUser@test.test", Password = "test", UserName = "TwwooGetAsyncWithTwoSortsUser" });
            await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "TwwooGetAsyncWithTwoSortsUser2@test.test", Password = "test", UserName = "GetAsyncWithTwoSorts2User" });
            await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "TwwooGetAsyncWithTwoSortsUser3@test.test", Password = "test", UserName = "GetAsyncWithTwoSorts3TwwooUser" });
            var user4 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "TwwooGetAsyncWithTwoSortsUser4@test.test", Password = "test", UserName = "AGetTwwooAsyncWithTwoSorts4User" });
            await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "TwwooGetAsyncWithTwoSortsUser5@test.test", Password = "test", UserName = "GetAsyncTwwooWithTwoSorts5User" });
            await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "TwwooGetAsyncWithTwoSortsUser6@test.test", Password = "test", UserName = "GetAsyncWithTwoSorts6User" });
            await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "TwwooGetAsyncWithTwoSortsUser7@test.test", Password = "test", UserName = "TGetAsyncWithTwoorts7User" });
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await userRepository.GetAsync(new UsernameContainsSpecification<User>("WithTwoSorts")
                                                            & new UsernameContainsSpecification<User>("Twwoo"), new PagingSpecification(0, 20),
                new SortSpecification<User>(new OrderBySpecification<User>(x => x.UserRoles.Count), SortingDirectionSpecification.Descending) &
                new SortSpecification<User>(new OrderBySpecification<User>(x => x.UserName), SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 4 && result.First().UserName == user4.Entity.UserName);
        }

        [Fact]
        public async void GetAsyncWithNoArgument()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);

            // Act & Assert
            Assert.True((await userRepository.GetAsync()).ToList().Count() == _fixture.Db.Users.Count());
        }

        [Fact]
        public async void GetAsyncWithAllArguments()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);
            var testUser = new User()
            {
                EmailAddress = "UserGetAsyncWithAllArguments@test.test", Password = "test", UserName = "UserGetAsyncWithAllArguments"
            };
            var testUser2 = new User()
            {
                EmailAddress = "UserGetAsyncWithAllArguments2@test.test", Password = "test", UserName = "UserGetAsyncAKWithAllArguments2"
            };
            var testUser3 = new User()
            {
                EmailAddress = "UserGetAsyncWithAllArguments3@test.test", Password = "test", UserName = "UserGetAsyncAkWithAllArguments3"
            };
            var testUser4 = new User()
            {
                EmailAddress = "UserGetAsyncWithAllArguments4@test.test", Password = "test", UserName = "UserGetAsyncWithAllArguments4"
            };
            var testUser5 = new User()
            {
                EmailAddress = "UserGetAsyncWithAllArguments5@test.test", Password = "test", UserName = "UserAKGetAsyncWithAllArguments5"
            };
            var testUser6 = new User()
            {
                EmailAddress = "UserGetAsyncWithAllArguments6@test.test", Password = "test", UserName = "UserGetAsyncWithAllArguments6"
            };
            await userRepository.AddAsync(testUser);
            await userRepository.AddAsync(testUser2);
            await userRepository.AddAsync(testUser3);
            await userRepository.AddAsync(testUser4);
            await userRepository.AddAsync(testUser5);
            await userRepository.AddAsync(testUser6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await userRepository.GetAsync(new UsernameContainsSpecification<User>("AK") &
                new UsernameContainsSpecification<User>("WithAllArguments"),
                new PagingSpecification(0, 2),
                new SortSpecification<User>(
                    new OrderBySpecification<User>(x => x.UserName),
                    SortingDirectionSpecification.Descending))).ToList();

            // Assert
            Assert.True(result.Count() == 2 && result.First().UserName == testUser2.UserName);
        }

        [Fact]
        public async void GetAsyncWithPagination()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);
            await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "UserGetAsyncWithPagination@test.test", Password = "test", UserName = "UserGetAsyncWithPagination" });
            var testUser = new User()
            {
                EmailAddress = "UserGetAsyncWithPagination1@test.test", Password = "test", UserName = "User1GetAsyncWithPagination1"
            };
            var testUser2 = new User()
            {
                EmailAddress = "UserGetAsyncWithPagination2@test.test", Password = "test", UserName = "User1GetAsyncWithPagination2"
            };
            var testUser3 = new User()
            {
                EmailAddress = "UserGetAsyncWithPagination3@test.test", Password = "test", UserName = "User1GetAsyncWithPagination3"
            };
            var testUser4 = new User()
            {
                EmailAddress = "UserGetAsyncWithPagination4@test.test", Password = "test", UserName = "User1GetAsyncWithPagination4"
            };
            var testUser5 = new User()
            {
                EmailAddress = "UserGetAsyncWithPagination5@test.test", Password = "test", UserName = "User1GetAsyncWithPagination5"
            };
            var testUser6 = new User()
            {
                EmailAddress = "UserGetAsyncWithPagination6@test.test", Password = "test", UserName = "User1GetAsyncWithPagination6"
            };
            await userRepository.AddAsync(testUser);
            await userRepository.AddAsync(testUser2);
            await userRepository.AddAsync(testUser3);
            await userRepository.AddAsync(testUser4);
            await userRepository.AddAsync(testUser5);
            await userRepository.AddAsync(testUser6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await userRepository.GetAsync(new UsernameContainsSpecification<User>("1GetAsyncWithPagination"),
                new PagingSpecification(2, 3),
                new SortSpecification<User>(
                    new OrderBySpecification<User>(x => x.UserName),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 3);
            Assert.Contains(result, x => x.Id == testUser3.Id &&
                                         x.UserName == testUser3.UserName);
            Assert.Contains(result, x => x.Id == testUser4.Id &&
                                         x.UserName == testUser4.UserName);
            Assert.Contains(result, x => x.Id == testUser5.Id &&
                                         x.UserName == testUser5.UserName);
        }

        [Fact]
        public async void GetAsyncWithPaginationTakeOutOfRange()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);
            var testUser = new User()
            {
                EmailAddress = "UserGetAsyncWithPaginationTakeOutOfRange1@test.test", Password = "test", UserName = "UserGetAsyncWithPaginationTakeOutOfRange1"
            };
            var testUser2 = new User()
            {
                EmailAddress = "UserGetAsyncWithPaginationTakeOutOfRange2@test.test", Password = "test", UserName = "UserGetAsyncWithPaginationTakeOutOfRange2"
            };
            var testUser3 = new User()
            {
                EmailAddress = "UserGetAsyncWithPaginationTakeOutOfRange3@test.test", Password = "test", UserName = "UserGetAsyncWithPaginationTakeOutOfRange3"
            };
            var testUser5 = new User()
            {
                EmailAddress = "UserGetAsyncWithPaginationTakeOutOfRange4@test.test", Password = "test", UserName = "UserGetAsyncWithPaginationTakeOutOfRange4"
            };
            var testUser6 = new User()
            {
                EmailAddress = "UserGetAsyncWithPaginationTakeOutOfRange5@test.test", Password = "test", UserName = "UserGetAsyncWithPaginationTakeOutOfRange5"
            };
            var testUser4 = new User()
            {
                EmailAddress = "UserGetAsyncWithPaginationTakeOutOfRange5@test.test", Password = "test", UserName = "UserGetAsyncWithPaginationTakeOutOfRange6"
            };
            await userRepository.AddAsync(testUser);
            await userRepository.AddAsync(testUser2);
            await userRepository.AddAsync(testUser3);
            await userRepository.AddAsync(testUser4);
            await userRepository.AddAsync(testUser5);
            await userRepository.AddAsync(testUser6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await userRepository.GetAsync(new UsernameContainsSpecification<User>("GetAsyncWithPaginationTakeOutOfRange"),
                new PagingSpecification(2, 22),
                new SortSpecification<User>(
                    new OrderBySpecification<User>(x => x.UserName),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 4);
            Assert.Contains(result, x => x.Id == testUser3.Id &&
                                         x.UserName == testUser3.UserName);
            Assert.Contains(result, x => x.Id == testUser4.Id &&
                                         x.UserName == testUser4.UserName);
            Assert.Contains(result, x => x.Id == testUser5.Id &&
                                         x.UserName == testUser5.UserName);
            Assert.Contains(result, x => x.Id == testUser6.Id &&
                                         x.UserName == testUser6.UserName);
        }

        [Fact]
        public async void GetAsyncWithPaginationTakeNegative()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);
            var testUser = new User()
            {
                EmailAddress = "UserGetAsyncWithPaginationTakeNegative1@test.test", Password = "test", UserName = "UserGetAsyncWithPaginationTakeNegative1"
            };
            var testUser2 = new User()
            {
                EmailAddress = "UserGetAsyncWithPaginationTakeNegative2@test.test", Password = "test", UserName = "UserGetAsyncWithPaginationTakeNegative2"
            };
            var testUser3 = new User()
            {
                EmailAddress = "UserGetAsyncWithPaginationTakeNegative3@test.test", Password = "test", UserName = "UserGetAsyncWithPaginationTakeNegative3"
            };
            var testUser5 = new User()
            {
                EmailAddress = "UserGetAsyncWithPaginationTakeNegative4@test.test", Password = "test", UserName = "UserGetAsyncWithPaginationTakeNegative4"
            };
            var testUser6 = new User()
            {
                EmailAddress = "UserGetAsyncWithPaginationTakeNegative5@test.test", Password = "test", UserName = "UserGetAsyncWithPaginationTakeNegative5"
            };
            var testUser4 = new User()
            {
                EmailAddress = "UserGetAsyncWithPaginationTakeNegative6@test.test", Password = "test", UserName = "UserGetAsyncWithPaginationTakeNegative6"
            };
            await userRepository.AddAsync(testUser);
            await userRepository.AddAsync(testUser2);
            await userRepository.AddAsync(testUser3);
            await userRepository.AddAsync(testUser4);
            await userRepository.AddAsync(testUser5);
            await userRepository.AddAsync(testUser6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await userRepository.GetAsync(new UsernameContainsSpecification<User>("GetAsyncWithPaginationTakeNegative"),
                new PagingSpecification(2, -2),
                new SortSpecification<User>(
                    new OrderBySpecification<User>(x => x.UserName),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(!result.Any());
        }

        [Fact]
        public async void GetAsyncWithPaginationSkipNegative()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);
            var testUser = new User()
            {
                EmailAddress = "UserGetAsyncWithPaginationSkipNegative@test.test", Password = "test", UserName = "UserGetAsyncWithPaginationSkipNegative"
            };
            var testUser2 = new User()
            {
                EmailAddress = "UserGetAsyncWithPaginationSkipNegative2@test.test", Password = "test", UserName = "UserGetAsyncWithPaginationSkipNegative2"
            };
            var testUser3 = new User()
            {
                EmailAddress = "UserGetAsyncWithPaginationSkipNegative3@test.test", Password = "test", UserName = "UserGetAsyncWithPaginationSkipNegative3"
            };
            var testUser4 = new User()
            {
                EmailAddress = "UserGetAsyncWithPaginationSkipNegative4@test.test", Password = "test", UserName = "UserGetAsyncWithPaginationSkipNegative4"
            };
            var testUser5 = new User()
            {
                EmailAddress = "UserGetAsyncWithPaginationSkipNegative5@test.test", Password = "test", UserName = "UserGetAsyncWithPaginationSkipSkipNegative5"
            };
            var testUser6 = new User()
            {
                EmailAddress = "UserGetAsyncWithPaginationSkipNegative6@test.test", Password = "test", UserName = "UserGetAsyncWithPaginationSkipSkipNegative6"
            };
            await userRepository.AddAsync(testUser);
            await userRepository.AddAsync(testUser2);
            await userRepository.AddAsync(testUser3);
            await userRepository.AddAsync(testUser4);
            await userRepository.AddAsync(testUser5);
            await userRepository.AddAsync(testUser6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await userRepository.GetAsync(new UsernameContainsSpecification<User>("GetAsyncWithPaginationSkipNegative"),
                new PagingSpecification(-2, 3),
                new SortSpecification<User>(
                    new OrderBySpecification<User>(x => x.UserName),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 3);
            Assert.Contains(result, x => x.Id == testUser.Id &&
                                         x.UserName == testUser.UserName);
            Assert.Contains(result, x => x.Id == testUser2.Id &&
                                         x.UserName == testUser2.UserName);
            Assert.Contains(result, x => x.Id == testUser3.Id &&
                                         x.UserName == testUser3.UserName);
        }

        [Fact]
        public async void GetAsyncWithPaginationSkipOutOfRange()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);
            var testUser = new User()
            {
                EmailAddress = "UserGetAsyncWithPaginationSkipOutOfRange@test.test", Password = "test", UserName = "UserGetAsyncWithPaginationSkipOutOfRange"
            };
            var testUser2 = new User()
            {
                EmailAddress = "UserGetAsyncWithPaginationSkipOutOfRange2@test.test", Password = "test", UserName = "UserGetAsyncWithPaginationSkipOutOfRange2"
            };
            var testUser3 = new User()
            {
                EmailAddress = "UserGetAsyncWithPaginationSkipOutOfRange3@test.test", Password = "test", UserName = "UserGetAsyncWithPaginationSkipOutOfRange3"
            };
            var testUser5 = new User()
            {
                EmailAddress = "UserGetAsyncWithPaginationSkipOutOfRange4@test.test", Password = "test", UserName = "UserGetAsyncWithPaginationSkipOutOfRange4"
            };
            var testUser6 = new User()
            {
                EmailAddress = "UserGetAsyncWithPaginationSkipOutOfRange5@test.test", Password = "test", UserName = "UserGetAsyncWithPaginationSkipOutOfRange5"
            };
            var testUser4 = new User()
            {
                EmailAddress = "UserGetAsyncWithPaginationSkipOutOfRange6@test.test", Password = "test", UserName = "UserGetAsyncWithPaginationSkipOutOfRange6"
            };
            await userRepository.AddAsync(testUser);
            await userRepository.AddAsync(testUser2);
            await userRepository.AddAsync(testUser3);
            await userRepository.AddAsync(testUser4);
            await userRepository.AddAsync(testUser5);
            await userRepository.AddAsync(testUser6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await userRepository.GetAsync(new UsernameContainsSpecification<User>("GetAsyncWithPaginationSkipOutOfRange"),
                new PagingSpecification(7, 3),
                new SortSpecification<User>(
                    new OrderBySpecification<User>(x => x.UserName),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(!result.Any());
        }

        [Fact]
        public void Remove()
        {
            // Arrange
            var nbCategoriesAtBeginning = _fixture.Db.Users.Count();
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);
            var testUser = new User()
            {
                EmailAddress = "RemoveUser@test.test", Password = "test", UserName = "RemoveUser"
            };
            userRepository.Add(testUser);
            _fixture.UnitOfWork.Save();
            var nbCategoriesAfterAdded = _fixture.Db.Users.Count();

            // Act
            userRepository.Remove(testUser);
            _fixture.UnitOfWork.Save();

            // Assert
            var nbCategoriesAfterRemoved = _fixture.Db.Users.Count();
            Assert.True(nbCategoriesAtBeginning + 1 == nbCategoriesAfterAdded &&
                        nbCategoriesAfterRemoved == nbCategoriesAtBeginning);
        }

        [Fact]
        public void GetUser()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);

            // Act
            var result = userRepository.Get(1);

            // Act & Assert
            Assert.True(result == _fixture.Db.Users.Find(1));
        }

        [Fact]
        public void GetCategoryOutOfRange()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<IndexOutOfRangeException>(() => userRepository.Get(100));
        }

        [Fact]
        public void RemoveNull()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => userRepository.Remove(null));
        }

        [Fact]
        public async void RemoveRangeAsyncNull()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await userRepository.RemoveRangeAsync(null));
        }

        [Fact]
        public void RemoveRangeNull()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => userRepository.RemoveRange(null));
        }

        [Fact]
        public void RemoveRange()
        {
            // Arrange
            var nbCategoriesAtBeginning = _fixture.Db.Users.Count();
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);
            var testUser = new User()
            {
                EmailAddress = "UserRemoveRange1@test.test", Password = "test", UserName = "UserRemoveRange1"
            };
            var testUser2 = new User()
            {
                EmailAddress = "UserRemoveRange2@test.test", Password = "test", UserName = "UserRemoveRange2"
            };
            userRepository.Add(testUser);
            userRepository.Add(testUser2);
            _fixture.UnitOfWork.Save();
            var nbCategoriesAfterAdded = _fixture.Db.Users.Count();

            // Act
            userRepository.RemoveRange(new List<User>() { testUser, testUser2 });
            _fixture.UnitOfWork.Save();

            // Assert
            var nbCategoriesAfterRemoved = _fixture.Db.Users.Count();
            Assert.True(nbCategoriesAtBeginning + 2 == nbCategoriesAfterAdded &&
                        nbCategoriesAfterRemoved == nbCategoriesAtBeginning);
        }

        [Fact]
        public async void RemoveRangeAsync()
        {
            // Arrange
            var nbCategoriesAtBeginning = _fixture.Db.Users.Count();
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);
            var testUser = new User()
            {
                EmailAddress = "UserRemoveRangeAsync1@test.test", Password = "test", UserName = "UserRemoveRangeAsync1"
            };
            var testUser2 = new User()
            {
                EmailAddress = "UserRemoveRangeAsync2@test.test", Password = "test", UserName = "UserRemoveRangeAsync2"
            };
            await userRepository.AddAsync(testUser);
            await userRepository.AddAsync(testUser2);
            _fixture.UnitOfWork.Save();
            var nbCategoriesAfterAdded = _fixture.Db.Users.Count();

            // Act
            await userRepository.RemoveRangeAsync(new List<User>() { testUser, testUser2 });
            _fixture.UnitOfWork.Save();

            // Assert
            var nbCategoriesAfterRemoved = _fixture.Db.Users.Count();
            Assert.True(nbCategoriesAtBeginning + 2 == nbCategoriesAfterAdded &&
                        nbCategoriesAfterRemoved == nbCategoriesAtBeginning);
        }

        [Fact]
        public async void UsernameAlreadyExistsFalse()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);

            // Act & Assert
            Assert.True(!await userRepository.UsernameAlreadyExists("UsernameAlreadyExistsFalse"));
        }

        [Fact]
        public async void EmailAddressAlreadyExistsFalse()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);

            // Act & Assert
            Assert.True(!await userRepository.EmailAddressAlreadyExists("EmailAddressAlreadyExistsFalse@test.test"));
        }

        [Fact]
        public async void UsernameAlreadyExistsNull()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);

            // Act & Assert
            Assert.True(!await userRepository.UsernameAlreadyExists(null));
        }

        [Fact]
        public async void EmailAddressAlreadyExistsNull()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);

            // Act & Assert
            Assert.True(!await userRepository.EmailAddressAlreadyExists(null));
        }

        [Fact]
        public async void UsernameAlreadyExistsTrue()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);
            var testUser = new User()
            {
                UserName = "UsernameAlreadyExistsValid",
                EmailAddress = "UsernameAlreadyExistsValid@test.test",
                Password = "test"
            };
            await userRepository.AddAsync(testUser);
            await _fixture.Db.SaveChangesAsync();

            // Act & Assert
            Assert.True(await userRepository.UsernameAlreadyExists(testUser.UserName));
        }

        [Fact]
        public async void EmailAddressAlreadyExistsTrue()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db);
            var testUser = new User()
            {
                UserName = "EmailAddressAlreadyExistsValid",
                EmailAddress = "EmailAddressAlreadyExistsValid@test.test",
                Password = "test"
            };
            await userRepository.AddAsync(testUser);
            await _fixture.Db.SaveChangesAsync();

            // Act & Assert
            Assert.True(await userRepository.EmailAddressAlreadyExists(testUser.EmailAddress));
        }
    }
}
