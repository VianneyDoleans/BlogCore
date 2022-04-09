using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbAccess.Data.POCO;
using DbAccess.Repositories;
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
        public async Task AddUserAsync()
        {
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var testUser = new User()
            {
                Email = "test@test.com",
                UserName = "test",
                Password = "testPassword-7"
            };
            await userRepository.AddAsync(testUser);
            _fixture.UnitOfWork.Save();

            Assert.True(_fixture.Db.Users.First(x => x.Email == testUser.Email &&
                                                     x.UserName == testUser.UserName) != null);
        }

        [Fact]
        public async Task AddNullUserAsync()
        {
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await userRepository.AddAsync(null));
        }

        [Fact]
        public async Task GetUserAsync()
        {
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var testUser = new User()
            {
                Email = "GetUserAsync@test.test",
                Password = "testA-007",
                UserName = "GetUserAsync"
            };
            await userRepository.AddAsync(testUser);
            _fixture.UnitOfWork.Save();

            var result = await userRepository.GetAsync(testUser.Id);
            
            Assert.True(result == await _fixture.Db.Users.FindAsync(testUser.Id));
        }

        [Fact]
        public async Task GetUserOutOfRangeAsync()
        {
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);

            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await userRepository.GetAsync(100));
        }

        [Fact]
        public async Task GetAllAsync()
        {
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var result = await userRepository.GetAllAsync();

            Assert.True(result.Count() == _fixture.Db.Users.Count());
        }

        [Fact]
        public async Task RemoveAsync()
        {
            var nbUsersAtBeginning = _fixture.Db.Users.Count();
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var testUser = new User()
            {
                Email = "test@test.com",
                UserName = "test",
                Password = "testPasswordA-007"
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
        public async Task RemoveNullAsync()
        {
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await userRepository.RemoveAsync(null));
        }

        // --------

        [Fact]
        public void AddUser()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            _fixture.Db.SaveChanges();
            var testUser = new User() { Email = "", Password = "test", UserName = "AddUser" };

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
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);

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
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);

            // Act & Assert
            Assert.True(_fixture.Db.Users.Count() == userRepository.CountAll());
        }

        [Fact]
        public async Task CountAllAsync()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);

            // Act & Assert
            Assert.True(_fixture.Db.Users.Count() == await userRepository.CountAllAsync());
        }


        [Fact]
        public void GetAll()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);

            // Act
            var result = userRepository.GetAll();

            // Assert
            Assert.True(result.Count() == _fixture.Db.Users.Count());
        }

        [Fact]
        public async Task GetAsyncSpecificationBasic()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            _fixture.Db.SaveChanges();
            var testUser = new User()
            {
                Email = "UserGetAsyncSpecification@test.test", Password = "test", UserName = "UserGetAsyncSpecification"
            };
            var testUser2 = new User()
            {
                Email = "UserGetAsyncSpecification2@test.test", Password = "test", UserName = "UserGetAsyncSpecification2"
            };
            var testUser3 = new User()
            {
                Email = "UserGetAsyncSpecification3@test.test", Password = "test", UserName = "UserGetAsyncSpecification3"
            };
            var testUser4 = new User()
            {
                Email = "UserGetAsyncSpecification4@test.test", Password = "test", UserName = "UserGetAsyncSpecification4"
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
        public async Task GetAsyncWithTwoSpecifications()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            _fixture.Db.SaveChanges();
            var testUser = new User()
            {
                Email = "GetAWiTwoSpecUser@test.test", Password = "testA-007", UserName = "GetAWiTwoSpecUser"
            };
            var testUser2 = new User()
            {
                Email = "GetAWiTwoSpecUser2@test.test", Password = "testA-007", UserName = "GetAWiTwoSpecAKUser2"
            };
            var testUser3 = new User()
            {
                Email = "GetAWiTwoSpecUser3@test.test", Password = "testA-007", UserName = "AKGetAWiTwoSpecUser3"
            };
            var testUser4 = new User()
            {
                Email = "GetAWiTwoSpecUser4@test.test", Password = "testA-007", UserName = "GetAWiTwoSpecPst4"
            };
            var testUser5 = new User()
            {
                Email = "GetAWiTwoSpecUser5@test.test", Password = "testA-007", UserName = "GetAWiTwoSpecUserAK5164"
            };
            var testUser6 = new User()
            {
                Email = "GetAWiTwoSpecUser6@test.test", Password = "testA-007", UserName = "GetAWiTwoSpecUser6"
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
        public async Task GetAsyncWithTwoSortsAndTwoSpecificationsAndPagination()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            await _fixture.Db.Users.AddAsync(
                new User() { Email = "TwwooGetAsyncWithTwoSortsUser@test.test", Password = "test", UserName = "TwwooGetAsyncWithTwoSortsUser" });
            await _fixture.Db.Users.AddAsync(
                new User() { Email = "TwwooGetAsyncWithTwoSortsUser2@test.test", Password = "test", UserName = "GetAsyncWithTwoSorts2User" });
            await _fixture.Db.Users.AddAsync(
                new User() { Email = "TwwooGetAsyncWithTwoSortsUser3@test.test", Password = "test", UserName = "GetAsyncWithTwoSorts3TwwooUser" });
            var user4 = await _fixture.Db.Users.AddAsync(
                new User() { Email = "TwwooGetAsyncWithTwoSortsUser4@test.test", Password = "test", UserName = "AGetTwwooAsyncWithTwoSorts4User" });
            await _fixture.Db.Users.AddAsync(
                new User() { Email = "TwwooGetAsyncWithTwoSortsUser5@test.test", Password = "test", UserName = "GetAsyncTwwooWithTwoSorts5User" });
            await _fixture.Db.Users.AddAsync(
                new User() { Email = "TwwooGetAsyncWithTwoSortsUser6@test.test", Password = "test", UserName = "GetAsyncWithTwoSorts6User" });
            await _fixture.Db.Users.AddAsync(
                new User() { Email = "TwwooGetAsyncWithTwoSortsUser7@test.test", Password = "test", UserName = "TGetAsyncWithTwoorts7User" });
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
        public async Task GetAsyncWithNoArgument()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);

            // Act & Assert
            Assert.True((await userRepository.GetAsync()).ToList().Count() == _fixture.Db.Users.Count());
        }

        [Fact]
        public async Task GetAsyncWithAllArguments()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var testUser = new User()
            {
                Email = "UserGetAsyncWithAllArguments@test.test", Password = "testA-7", UserName = "UserGetAsyncWithAllArguments"
            };
            var testUser2 = new User()
            {
                Email = "UserGetAsyncWithAllArguments2@test.test", Password = "testA-7", UserName = "UserGetAsyncAKWithAllArguments2"
            };
            var testUser3 = new User()
            {
                Email = "UserGetAsyncWithAllArguments3@test.test", Password = "testA-7", UserName = "UserGetAsyncAkWithAllArguments3"
            };
            var testUser4 = new User()
            {
                Email = "UserGetAsyncWithAllArguments4@test.test", Password = "testA-7", UserName = "UserGetAsyncWithAllArguments4"
            };
            var testUser5 = new User()
            {
                Email = "UserGetAsyncWithAllArguments5@test.test", Password = "testA-7", UserName = "UserAKGetAsyncWithAllArguments5"
            };
            var testUser6 = new User()
            {
                Email = "UserGetAsyncWithAllArguments6@test.test", Password = "testA-7", UserName = "UserGetAsyncWithAllArguments6"
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
        public async Task GetAsyncWithPagination()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            await _fixture.Db.Users.AddAsync(
                new User() { Email = "UserGetAsyncWithPagination@test.test", Password = "testA-7", UserName = "UserGetAsyncWithPagination" });
            var testUser = new User()
            {
                Email = "UserGetAsyncWithPagination1@test.test", Password = "testA-007", UserName = "User1GetAsyncWithPagination1"
            };
            var testUser2 = new User()
            {
                Email = "UserGetAsyncWithPagination2@test.test", Password = "testA-007", UserName = "User1GetAsyncWithPagination2"
            };
            var testUser3 = new User()
            {
                Email = "UserGetAsyncWithPagination3@test.test", Password = "testA-007", UserName = "User1GetAsyncWithPagination3"
            };
            var testUser4 = new User()
            {
                Email = "UserGetAsyncWithPagination4@test.test", Password = "testA-007", UserName = "User1GetAsyncWithPagination4"
            };
            var testUser5 = new User()
            {
                Email = "UserGetAsyncWithPagination5@test.test", Password = "testA-007", UserName = "User1GetAsyncWithPagination5"
            };
            var testUser6 = new User()
            {
                Email = "UserGetAsyncWithPagination6@test.test", Password = "testA-007", UserName = "User1GetAsyncWithPagination6"
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
        public async Task GetAsyncWithPaginationTakeOutOfRange()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var testUser = new User()
            {
                Email = "UserGetAsyncWithPaginationTakeOutOfRange1@test.test", Password = "testA-007", UserName = "UserGetAsyncWithPaginationTakeOutOfRange1"
            };
            var testUser2 = new User()
            {
                Email = "UserGetAsyncWithPaginationTakeOutOfRange2@test.test", Password = "testA-007", UserName = "UserGetAsyncWithPaginationTakeOutOfRange2"
            };
            var testUser3 = new User()
            {
                Email = "UserGetAsyncWithPaginationTakeOutOfRange3@test.test", Password = "testA-007", UserName = "UserGetAsyncWithPaginationTakeOutOfRange3"
            };
            var testUser5 = new User()
            {
                Email = "UserGetAsyncWithPaginationTakeOutOfRange4@test.test", Password = "testA-007", UserName = "UserGetAsyncWithPaginationTakeOutOfRange4"
            };
            var testUser6 = new User()
            {
                Email = "UserGetAsyncWithPaginationTakeOutOfRange5@test.test", Password = "testA-007", UserName = "UserGetAsyncWithPaginationTakeOutOfRange5"
            };
            var testUser4 = new User()
            {
                Email = "UserGetAsyncWithPaginationTakeOutOfRange5@test.test", Password = "testA-007", UserName = "UserGetAsyncWithPaginationTakeOutOfRange6"
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
        public async Task GetAsyncWithPaginationTakeNegative()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var testUser = new User()
            {
                Email = "UserGetAsyncWithPaginationTakeNegative1@test.test", Password = "testA-007", UserName = "UserGetAsyncWithPaginationTakeNegative1"
            };
            var testUser2 = new User()
            {
                Email = "UserGetAsyncWithPaginationTakeNegative2@test.test", Password = "testA-007", UserName = "UserGetAsyncWithPaginationTakeNegative2"
            };
            var testUser3 = new User()
            {
                Email = "UserGetAsyncWithPaginationTakeNegative3@test.test", Password = "testA-007", UserName = "UserGetAsyncWithPaginationTakeNegative3"
            };
            var testUser5 = new User()
            {
                Email = "UserGetAsyncWithPaginationTakeNegative4@test.test", Password = "testA-007", UserName = "UserGetAsyncWithPaginationTakeNegative4"
            };
            var testUser6 = new User()
            {
                Email = "UserGetAsyncWithPaginationTakeNegative5@test.test", Password = "testA-007", UserName = "UserGetAsyncWithPaginationTakeNegative5"
            };
            var testUser4 = new User()
            {
                Email = "UserGetAsyncWithPaginationTakeNegative6@test.test", Password = "testA-007", UserName = "UserGetAsyncWithPaginationTakeNegative6"
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
        public async Task GetAsyncWithPaginationSkipNegative()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var testUser = new User()
            {
                Email = "UserGetAsyncWithPaginationSkipNegative@test.test", Password = "testA-007", UserName = "UserGetAsyncWithPaginationSkipNegative"
            };
            var testUser2 = new User()
            {
                Email = "UserGetAsyncWithPaginationSkipNegative2@test.test", Password = "testA-007", UserName = "UserGetAsyncWithPaginationSkipNegative2"
            };
            var testUser3 = new User()
            {
                Email = "UserGetAsyncWithPaginationSkipNegative3@test.test", Password = "testA-007", UserName = "UserGetAsyncWithPaginationSkipNegative3"
            };
            var testUser4 = new User()
            {
                Email = "UserGetAsyncWithPaginationSkipNegative4@test.test", Password = "testA-007", UserName = "UserGetAsyncWithPaginationSkipNegative4"
            };
            var testUser5 = new User()
            {
                Email = "UserGetAsyncWithPaginationSkipNegative5@test.test", Password = "testA-007", UserName = "UserGetAsyncWithPaginationSkipSkipNegative5"
            };
            var testUser6 = new User()
            {
                Email = "UserGetAsyncWithPaginationSkipNegative6@test.test", Password = "testA-007", UserName = "UserGetAsyncWithPaginationSkipSkipNegative6"
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
        public async Task GetAsyncWithPaginationSkipOutOfRange()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var testUser = new User()
            {
                Email = "UserGetAsyncWithPaginationSkipOutOfRange@test.test", Password = "testA-007", UserName = "UserGetAsyncWithPaginationSkipOutOfRange"
            };
            var testUser2 = new User()
            {
                Email = "UserGetAsyncWithPaginationSkipOutOfRange2@test.test", Password = "testA-007", UserName = "UserGetAsyncWithPaginationSkipOutOfRange2"
            };
            var testUser3 = new User()
            {
                Email = "UserGetAsyncWithPaginationSkipOutOfRange3@test.test", Password = "testA-007", UserName = "UserGetAsyncWithPaginationSkipOutOfRange3"
            };
            var testUser5 = new User()
            {
                Email = "UserGetAsyncWithPaginationSkipOutOfRange4@test.test", Password = "testA-007", UserName = "UserGetAsyncWithPaginationSkipOutOfRange4"
            };
            var testUser6 = new User()
            {
                Email = "UserGetAsyncWithPaginationSkipOutOfRange5@test.test", Password = "testA-007", UserName = "UserGetAsyncWithPaginationSkipOutOfRange5"
            };
            var testUser4 = new User()
            {
                Email = "UserGetAsyncWithPaginationSkipOutOfRange6@test.test", Password = "testA-007", UserName = "UserGetAsyncWithPaginationSkipOutOfRange6"
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
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var testUser = new User()
            {
                Email = "RemoveUser@test.test", Password = "test", UserName = "RemoveUser"
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
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var testUser = new User()
            {
                Email = "GetUser@test.test",
                Password = "test",
                UserName = "GetUser"
            };
            userRepository.Add(testUser);
            _fixture.UnitOfWork.Save();

            // Act
            var result = userRepository.Get(testUser.Id);

            // Act & Assert
            Assert.True(result == _fixture.Db.Users.Find(testUser.Id));
        }

        [Fact]
        public void GetCategoryOutOfRange()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);

            // Act & Assert
            Assert.Throws<IndexOutOfRangeException>(() => userRepository.Get(100));
        }

        [Fact]
        public void RemoveNull()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => userRepository.Remove(null));
        }

        [Fact]
        public async Task RemoveRangeAsyncNull()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await userRepository.RemoveRangeAsync(null));
        }

        [Fact]
        public void RemoveRangeNull()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => userRepository.RemoveRange(null));
        }

        [Fact]
        public void RemoveRange()
        {
            // Arrange
            var nbCategoriesAtBeginning = _fixture.Db.Users.Count();
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var testUser = new User()
            {
                Email = "UserRemoveRange1@test.test", Password = "test", UserName = "UserRemoveRange1"
            };
            var testUser2 = new User()
            {
                Email = "UserRemoveRange2@test.test", Password = "test", UserName = "UserRemoveRange2"
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
        public async Task RemoveRangeAsync()
        {
            // Arrange
            var nbCategoriesAtBeginning = _fixture.Db.Users.Count();
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var testUser = new User()
            {
                Email = "UserRemoveRangeAsync1@test.test", Password = "testA-007", UserName = "UserRemoveRangeAsync1"
            };
            var testUser2 = new User()
            {
                Email = "UserRemoveRangeAsync2@test.test", Password = "testA-007", UserName = "UserRemoveRangeAsync2"
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
        public async Task UsernameAlreadyExistsFalse()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);

            // Act & Assert
            Assert.True(!await userRepository.UserNameAlreadyExists("UsernameAlreadyExistsFalse"));
        }

        [Fact]
        public async Task EmailAddressAlreadyExistsFalse()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);

            // Act & Assert
            Assert.True(!await userRepository.EmailAlreadyExists("EmailAddressAlreadyExistsFalse@test.test"));
        }

        [Fact]
        public async Task UsernameAlreadyExistsNull()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);

            // Act & Assert
            Assert.True(!await userRepository.UserNameAlreadyExists(null));
        }

        [Fact]
        public async Task EmailAddressAlreadyExistsNull()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);

            // Act & Assert
            Assert.True(!await userRepository.EmailAlreadyExists(null));
        }

        [Fact]
        public async Task UsernameAlreadyExistsTrue()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var testUser = new User()
            {
                UserName = "UsernameAlreadyExistsValid",
                Email = "UsernameAlreadyExistsValid@test.test",
                Password = "testA-007"
            };
            await userRepository.AddAsync(testUser);
            await _fixture.Db.SaveChangesAsync();

            // Act & Assert
            Assert.True(await userRepository.UserNameAlreadyExists(testUser.UserName));
        }

        [Fact]
        public async Task EmailAddressAlreadyExistsTrue()
        {
            // Arrange
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var testUser = new User()
            {
                UserName = "EmailAddressAlreadyExistsValid",
                Email = "EmailAddressAlreadyExistsValid@test.test",
                Password = "testA-7"
            };
            await userRepository.AddAsync(testUser);
            await _fixture.Db.SaveChangesAsync();

            // Act & Assert
            Assert.True(await userRepository.EmailAlreadyExists(testUser.Email));
        }
    }
}
