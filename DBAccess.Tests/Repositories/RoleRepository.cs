﻿using System;
using System.Collections.Generic;
using System.Linq;
using DbAccess.Data.POCO;
using DbAccess.Specifications;
using DbAccess.Specifications.FilterSpecifications.Filters;
using DbAccess.Specifications.SortSpecification;
using Xunit;

namespace DBAccess.Tests.Repositories
{
    public class RolesRepository
    {
        private readonly DatabaseFixture _fixture;

        public RolesRepository()
        {
            _fixture = new DatabaseFixture();
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

            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await repository.GetAsync(100));
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
            var roleRepository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);
            var testRoles = new Role() { Name = "This is a test role" };

            await roleRepository.AddAsync(testRoles);
            _fixture.UnitOfWork.Save();
            var nbRolesAfterAdded = _fixture.Db.Roles.Count();
            await roleRepository.RemoveAsync(testRoles);
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

        // --------

        [Fact]
        public void AddRole()
        {
            // Arrange
            var roleRepository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);
            _fixture.Db.SaveChanges();
            var testRole = new Role() { Name = "AddRole" };

            // Act
            roleRepository.Add(testRole);
            _fixture.UnitOfWork.Save();

            // Assert
            Assert.True(_fixture.Db.Roles.First(x => x.Name == "AddRole") != null);
        }

        [Fact]
        public void AddNullRole()
        {
            // Arrange
            var roleRepository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                roleRepository.Add(null);
                _fixture.Db.SaveChanges();
            });
        }

        [Fact]
        public void CountAll()
        {
            // Arrange
            var roleRepository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);

            // Act & Assert
            Assert.True(_fixture.Db.Roles.Count() == roleRepository.CountAll());
        }

        [Fact]
        public async void CountAllAsync()
        {
            // Arrange
            var roleRepository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);

            // Act & Assert
            Assert.True(_fixture.Db.Roles.Count() == await roleRepository.CountAllAsync());
        }


        [Fact]
        public void GetAll()
        {
            // Arrange
            var roleRepository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);

            // Act
            var result = roleRepository.GetAll();

            // Assert
            Assert.True(result.Count() == _fixture.Db.Roles.Count());
        }

        [Fact]
        public async void GetAsyncSpecificationBasic()
        {
            // Arrange
            var roleRepository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);
            _fixture.Db.SaveChanges();
            var testRole = new Role()
            {
                Name = "RoleGetAsyncSpecification"
            };
            var testRole2 = new Role()
            {
                Name = "RoleGetAsyncSpecification2"
            };
            var testRole3 = new Role()
            {
                Name = "RoleGetAsyncSpecification3"
            };
            var testRole4 = new Role()
            {
                Name = "RoleGetAsyncSpecification4"
            };
            roleRepository.Add(testRole);
            roleRepository.Add(testRole2);
            roleRepository.Add(testRole3);
            roleRepository.Add(testRole4);
            _fixture.UnitOfWork.Save();

            // Act
            var result = await roleRepository.GetAsync(new IdSpecification<Role>(testRole2.Id));

            // Assert
            Assert.True(result.First().Id == testRole2.Id);
        }

        [Fact]
        public async void GetAsyncWithTwoSpecifications()
        {
            // Arrange
            var roleRepository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);
            _fixture.Db.SaveChanges();
            var testRole = new Role()
            {
                Name = "GetAWiTwoSpecRole"
            };
            var testRole2 = new Role()
            {
                Name = "GetAWiTwoSpecAKRole2"
            };
            var testRole3 = new Role()
            {
                Name = "AKGetAWiTwoSpecRole3"
            };
            var testRole4 = new Role()
            {
                Name = "GetAWiTwoSpecPst4"
            };
            var testRole5 = new Role()
            {
                Name = "GetAWiTwoSpecRoleAK5164"
            };
            var testRole6 = new Role()
            {
                Name = "GetAWiTwoSpecRole6"
            };
            await roleRepository.AddAsync(testRole);
            await roleRepository.AddAsync(testRole2);
            await roleRepository.AddAsync(testRole3);
            await roleRepository.AddAsync(testRole4);
            await roleRepository.AddAsync(testRole5);
            await roleRepository.AddAsync(testRole6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await roleRepository.GetAsync(new NameContainsSpecification<Role>("AK") & new NameContainsSpecification<Role>("164"))).ToList();

            // Assert
            Assert.True(result.Count() == 1 && result.First().Name == testRole5.Name);
        }

        [Fact]
        public async void GetAsyncWithTwoSortsAndTwoSpecificationsAndPagination()
        {
            // Arrange
            var roleRepository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);
            await _fixture.Db.Roles.AddAsync(
                new Role() { Name = "TwwooGetAsyncWithTwoSortsRole" });
            await _fixture.Db.Roles.AddAsync(
                new Role() { Name = "GetAsyncWithTwoSorts2Role" });
            await _fixture.Db.Roles.AddAsync(
                new Role() { Name = "GetAsyncWithTwoSorts3TwwooRole" });
            var role4 = await _fixture.Db.Roles.AddAsync(
                new Role() { Name = "AGetTwwooAsyncWithTwoSorts4Role" });
            await _fixture.Db.Roles.AddAsync(
                new Role() { Name = "GetAsyncTwwooWithTwoSorts5Role" });
            await _fixture.Db.Roles.AddAsync(
                new Role() { Name = "GetAsyncWithTwoSorts6Role" });
            await _fixture.Db.Roles.AddAsync(
                new Role() { Name = "TGetAsyncWithTwoorts7Role" });
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await roleRepository.GetAsync(new NameContainsSpecification<Role>("WithTwoSorts")
                                                            & new NameContainsSpecification<Role>("Twwoo"), new PagingSpecification(0, 20),
                new SortSpecification<Role>(new OrderBySpecification<Role>(x => x.UserRoles.Count), SortingDirectionSpecification.Descending) &
                new SortSpecification<Role>(new OrderBySpecification<Role>(x => x.Name), SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 4 && result.First().Name == role4.Entity.Name);
        }

        [Fact]
        public async void GetAsyncWithNoArgument()
        {
            // Arrange
            var roleRepository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);

            // Act & Assert
            Assert.True((await roleRepository.GetAsync()).ToList().Count() == _fixture.Db.Roles.Count());
        }

        [Fact]
        public async void GetAsyncWithAllArguments()
        {
            // Arrange
            var roleRepository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);
            var testRole = new Role()
            {
                Name = "RoleGetAsyncWithAllArguments"
            };
            var testRole2 = new Role()
            {
                Name = "RoleGetAsyncAKWithAllArguments2"
            };
            var testRole3 = new Role()
            {
                Name = "RoleGetAsyncAkWithAllArguments3"
            };
            var testRole4 = new Role()
            {
                Name = "RoleGetAsyncWithAllArguments4"
            };
            var testRole5 = new Role()
            {
                Name = "RoleAKGetAsyncWithAllArguments5"
            };
            var testRole6 = new Role()
            {
                Name = "RoleGetAsyncWithAllArguments6"
            };
            await roleRepository.AddAsync(testRole);
            await roleRepository.AddAsync(testRole2);
            await roleRepository.AddAsync(testRole3);
            await roleRepository.AddAsync(testRole4);
            await roleRepository.AddAsync(testRole5);
            await roleRepository.AddAsync(testRole6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await roleRepository.GetAsync(new NameContainsSpecification<Role>("AK") &
                new NameContainsSpecification<Role>("WithAllArguments"),
                new PagingSpecification(0, 2),
                new SortSpecification<Role>(
                    new OrderBySpecification<Role>(x => x.Name),
                    SortingDirectionSpecification.Descending))).ToList();

            // Assert
            Assert.True(result.Count() == 2 && result.First().Name == testRole2.Name);
        }

        [Fact]
        public async void GetAsyncWithPagination()
        {
            // Arrange
            var roleRepository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);
            await _fixture.Db.Roles.AddAsync(
                new Role() { Name = "GetAsyncWithPagination" });
            var testRole = new Role()
            {
                Name = "Role1GetAsyncWithPagination1"
            };
            var testRole2 = new Role()
            {
                Name = "Role1GetAsyncWithPagination2"
            };
            var testRole3 = new Role()
            {
                Name = "Role1GetAsyncWithPagination3"
            };
            var testRole4 = new Role()
            {
                Name = "Role1GetAsyncWithPagination4"
            };
            var testRole5 = new Role()
            {
                Name = "Role1GetAsyncWithPagination5"
            };
            var testRole6 = new Role()
            {
                Name = "Role1GetAsyncWithPagination6"
            };
            await roleRepository.AddAsync(testRole);
            await roleRepository.AddAsync(testRole2);
            await roleRepository.AddAsync(testRole3);
            await roleRepository.AddAsync(testRole4);
            await roleRepository.AddAsync(testRole5);
            await roleRepository.AddAsync(testRole6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await roleRepository.GetAsync(new NameContainsSpecification<Role>("1GetAsyncWithPagination"),
                new PagingSpecification(2, 3),
                new SortSpecification<Role>(
                    new OrderBySpecification<Role>(x => x.Name),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 3);
            Assert.Contains(result, x => x.Id == testRole3.Id &&
                                         x.Name == testRole3.Name);
            Assert.Contains(result, x => x.Id == testRole4.Id &&
                                         x.Name == testRole4.Name);
            Assert.Contains(result, x => x.Id == testRole5.Id &&
                                         x.Name == testRole5.Name);
        }

        [Fact]
        public async void GetAsyncWithPaginationTakeOutOfRange()
        {
            // Arrange
            var roleRepository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);
            var testRole = new Role()
            {
                Name = "RoleGetAsyncWithPaginationTakeOutOfRange1"
            };
            var testRole2 = new Role()
            {
                Name = "RoleGetAsyncWithPaginationTakeOutOfRange2"
            };
            var testRole3 = new Role()
            {
                Name = "RoleGetAsyncWithPaginationTakeOutOfRange3"
            };
            var testRole5 = new Role()
            {
                Name = "RoleGetAsyncWithPaginationTakeOutOfRange4"
            };
            var testRole6 = new Role()
            {
                Name = "RoleGetAsyncWithPaginationTakeOutOfRange5"
            };
            var testRole4 = new Role()
            {
                Name = "RoleGetAsyncWithPaginationTakeOutOfRange6"
            };
            await roleRepository.AddAsync(testRole);
            await roleRepository.AddAsync(testRole2);
            await roleRepository.AddAsync(testRole3);
            await roleRepository.AddAsync(testRole4);
            await roleRepository.AddAsync(testRole5);
            await roleRepository.AddAsync(testRole6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await roleRepository.GetAsync(new NameContainsSpecification<Role>("GetAsyncWithPaginationTakeOutOfRange"),
                new PagingSpecification(2, 22),
                new SortSpecification<Role>(
                    new OrderBySpecification<Role>(x => x.Name),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 4);
            Assert.Contains(result, x => x.Id == testRole3.Id &&
                                         x.Name == testRole3.Name);
            Assert.Contains(result, x => x.Id == testRole4.Id &&
                                         x.Name == testRole4.Name);
            Assert.Contains(result, x => x.Id == testRole5.Id &&
                                         x.Name == testRole5.Name);
            Assert.Contains(result, x => x.Id == testRole6.Id &&
                                         x.Name == testRole6.Name);
        }

        [Fact]
        public async void GetAsyncWithPaginationTakeNegative()
        {
            // Arrange
            var roleRepository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);
            var testRole = new Role()
            {
                Name = "RoleGetAsyncWithPaginationTakeNegative1"
            };
            var testRole2 = new Role()
            {
                Name = "RoleGetAsyncWithPaginationTakeNegative2"
            };
            var testRole3 = new Role()
            {
                Name = "RoleGetAsyncWithPaginationTakeNegative3"
            };
            var testRole5 = new Role()
            {
                Name = "RoleGetAsyncWithPaginationTakeNegative4"
            };
            var testRole6 = new Role()
            {
                Name = "RoleGetAsyncWithPaginationTakeNegative5"
            };
            var testRole4 = new Role()
            {
                Name = "RoleGetAsyncWithPaginationTakeNegative6"
            };
            await roleRepository.AddAsync(testRole);
            await roleRepository.AddAsync(testRole2);
            await roleRepository.AddAsync(testRole3);
            await roleRepository.AddAsync(testRole4);
            await roleRepository.AddAsync(testRole5);
            await roleRepository.AddAsync(testRole6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await roleRepository.GetAsync(new NameContainsSpecification<Role>("GetAsyncWithPaginationTakeNegative"),
                new PagingSpecification(2, -2),
                new SortSpecification<Role>(
                    new OrderBySpecification<Role>(x => x.Name),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(!result.Any());
        }

        [Fact]
        public async void GetAsyncWithPaginationSkipNegative()
        {
            // Arrange
            var roleRepository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);
            await _fixture.Db.Roles.AddAsync(
                new Role() { Name = "ComGetAWithPagSkipNega" });
            var testRole = new Role()
            {
                Name = "RoleGetAsyncWithPaginationSkipNegative"
            };
            var testRole2 = new Role()
            {
                Name = "RoleGetAsyncWithPaginationSkipNegative2"
            };
            var testRole3 = new Role()
            {
                Name = "RoleGetAsyncWithPaginationSkipNegative3"
            };
            var testRole4 = new Role()
            {
                Name = "RoleGetAsyncWithPaginationSkipNegative4"
            };
            var testRole5 = new Role()
            {
                Name = "RoleGetAsyncWithPaginationSkipSkipNegative5"
            };
            var testRole6 = new Role()
            {
                Name = "RoleGetAsyncWithPaginationSkipSkipNegative6"
            };
            await roleRepository.AddAsync(testRole);
            await roleRepository.AddAsync(testRole2);
            await roleRepository.AddAsync(testRole3);
            await roleRepository.AddAsync(testRole4);
            await roleRepository.AddAsync(testRole5);
            await roleRepository.AddAsync(testRole6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await roleRepository.GetAsync(new NameContainsSpecification<Role>("GetAsyncWithPaginationSkipNegative"),
                new PagingSpecification(-2, 3),
                new SortSpecification<Role>(
                    new OrderBySpecification<Role>(x => x.Name),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 3);
            Assert.Contains(result, x => x.Id == testRole.Id &&
                                         x.Name == testRole.Name);
            Assert.Contains(result, x => x.Id == testRole2.Id &&
                                         x.Name == testRole2.Name);
            Assert.Contains(result, x => x.Id == testRole3.Id &&
                                         x.Name == testRole3.Name);
        }

        [Fact]
        public async void GetAsyncWithPaginationSkipOutOfRange()
        {
            // Arrange
            var roleRepository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);
            var testRole = new Role()
            {
                Name = "RoleGetAsyncWithPaginationSkipOutOfRange"
            };
            var testRole2 = new Role()
            {
                Name = "RoleGetAsyncWithPaginationSkipOutOfRange2"
            };
            var testRole3 = new Role()
            {
                Name = "RoleGetAsyncWithPaginationSkipOutOfRange3"
            };
            var testRole5 = new Role()
            {
                Name = "RoleGetAsyncWithPaginationSkipOutOfRange4"
            };
            var testRole6 = new Role()
            {
                Name = "RoleGetAsyncWithPaginationSkipOutOfRange5"
            };
            var testRole4 = new Role()
            {
                Name = "RoleGetAsyncWithPaginationSkipOutOfRange6"
            };
            await roleRepository.AddAsync(testRole);
            await roleRepository.AddAsync(testRole2);
            await roleRepository.AddAsync(testRole3);
            await roleRepository.AddAsync(testRole4);
            await roleRepository.AddAsync(testRole5);
            await roleRepository.AddAsync(testRole6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await roleRepository.GetAsync(new NameContainsSpecification<Role>("GetAsyncWithPaginationSkipOutOfRange"),
                new PagingSpecification(7, 3),
                new SortSpecification<Role>(
                    new OrderBySpecification<Role>(x => x.Name),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(!result.Any());
        }

        [Fact]
        public void Remove()
        {
            // Arrange
            var nbCategoriesAtBeginning = _fixture.Db.Roles.Count();
            var roleRepository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);
            var testRole = new Role()
            {
                Name = "RemoveRole"
            };
            roleRepository.Add(testRole);
            _fixture.UnitOfWork.Save();
            var nbCategoriesAfterAdded = _fixture.Db.Roles.Count();

            // Act
            roleRepository.Remove(testRole);
            _fixture.UnitOfWork.Save();

            // Assert
            var nbCategoriesAfterRemoved = _fixture.Db.Roles.Count();
            Assert.True(nbCategoriesAtBeginning + 1 == nbCategoriesAfterAdded &&
                        nbCategoriesAfterRemoved == nbCategoriesAtBeginning);
        }

        [Fact]
        public void GetRole()
        {
            // Arrange
            var roleRepository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);

            // Act
            var result = roleRepository.Get(1);

            // Act & Assert
            Assert.True(result == _fixture.Db.Roles.Find(1));
        }

        [Fact]
        public void GetCategoryOutOfRange()
        {
            // Arrange
            var roleRepository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<IndexOutOfRangeException>(() => roleRepository.Get(100));
        }

        [Fact]
        public void RemoveNull()
        {
            // Arrange
            var roleRepository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => roleRepository.Remove(null));
        }

        [Fact]
        public async void RemoveRangeAsyncNull()
        {
            // Arrange
            var roleRepository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await roleRepository.RemoveRangeAsync(null));
        }

        [Fact]
        public void RemoveRangeNull()
        {
            // Arrange
            var roleRepository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => roleRepository.RemoveRange(null));
        }

        [Fact]
        public void RemoveRange()
        {
            // Arrange
            var nbCategoriesAtBeginning = _fixture.Db.Roles.Count();
            var roleRepository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);
            var testRole = new Role()
            {
                Name = "RoleRemoveRange1"
            };
            var testRole2 = new Role()
            {
                Name = "RoleRemoveRange2"
            };
            roleRepository.Add(testRole);
            roleRepository.Add(testRole2);
            _fixture.UnitOfWork.Save();
            var nbCategoriesAfterAdded = _fixture.Db.Roles.Count();

            // Act
            roleRepository.RemoveRange(new List<Role>() { testRole, testRole2 });
            _fixture.UnitOfWork.Save();

            // Assert
            var nbCategoriesAfterRemoved = _fixture.Db.Roles.Count();
            Assert.True(nbCategoriesAtBeginning + 2 == nbCategoriesAfterAdded &&
                        nbCategoriesAfterRemoved == nbCategoriesAtBeginning);
        }

        [Fact]
        public async void RemoveRangeAsync()
        {
            // Arrange
            var nbCategoriesAtBeginning = _fixture.Db.Roles.Count();
            var roleRepository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);
            var testRole = new Role()
            {
                Name = "RoleRemoveRangeAsync1"
            };
            var testRole2 = new Role()
            {
                Name = "RoleRemoveRangeAsync2"
            };
            await roleRepository.AddAsync(testRole);
            await roleRepository.AddAsync(testRole2);
            _fixture.UnitOfWork.Save();
            var nbCategoriesAfterAdded = _fixture.Db.Roles.Count();

            // Act
            await roleRepository.RemoveRangeAsync(new List<Role>() { testRole, testRole2 });
            _fixture.UnitOfWork.Save();

            // Assert
            var nbCategoriesAfterRemoved = _fixture.Db.Roles.Count();
            Assert.True(nbCategoriesAtBeginning + 2 == nbCategoriesAfterAdded &&
                        nbCategoriesAfterRemoved == nbCategoriesAtBeginning);
        }

        [Fact]
        public async void NameAlreadyExistsFalse()
        {
            // Arrange
            var roleRepository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);

            // Act & Assert
            Assert.True(!await roleRepository.NameAlreadyExists("NameAlreadyExistsFalse"));
        }

        [Fact]
        public async void NameAlreadyExistsNull()
        {
            // Arrange
            var roleRepository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);

            // Act & Assert
            Assert.True(!await roleRepository.NameAlreadyExists(null));
        }

        [Fact]
        public async void NameAlreadyExistsTrue()
        {
            // Arrange
            var roleRepository = new DbAccess.Repositories.Role.RoleRepository(_fixture.Db);
            var testRole = new Role()
            {
                Name = "NameAlreadyExistsTrue",
            };
            await roleRepository.AddAsync(testRole);
            await _fixture.Db.SaveChangesAsync();

            // Act & Assert
            Assert.True(await roleRepository.NameAlreadyExists(testRole.Name));
        }
    }
}