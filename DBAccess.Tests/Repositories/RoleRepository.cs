using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DBAccess.Data;
using DBAccess.Data.Permission;
using DBAccess.Repositories.Role;
using DBAccess.Specifications;
using DBAccess.Specifications.FilterSpecifications.Filters;
using DBAccess.Specifications.SortSpecification;
using DBAccess.Tests.Builders;
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
        public async Task AddRolesAsync()
        {
            var repository = new RoleRepository(_fixture.Db, _fixture.RoleManager);
            var testRoles = new Role() { Name = "This is a test for role" };
            await repository.AddAsync(testRoles);
            _fixture.UnitOfWork.Save();

            Assert.True(_fixture.Db.Roles.First(x => x.Name == testRoles.Name) != null);
        }

        [Fact]
        public async Task AddNullRolesAsync()
        {
            var repository = new RoleRepository(_fixture.Db, _fixture.RoleManager);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.AddAsync(null));
        }

        [Fact]
        public async Task GetRolesAsync()
        {
            var repository = new RoleRepository(_fixture.Db, _fixture.RoleManager);
            var testRole = new Role()
            {
                Name = "GetRolesAsync"
            };
            await repository.AddAsync(testRole);
            _fixture.UnitOfWork.Save();

            var result = await repository.GetAsync(testRole.Id);

            Assert.True(result == await _fixture.Db.Roles.FindAsync(testRole.Id));
        }

        [Fact]
        public async Task GetRolesOutOfRangeAsync()
        {
            var repository = new RoleRepository(_fixture.Db, _fixture.RoleManager);

            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await repository.GetAsync(100));
        }

        [Fact]
        public async Task GetAllAsync()
        {
            var repository = new RoleRepository(_fixture.Db, _fixture.RoleManager);
            var result = await repository.GetAllAsync();

            Assert.True(result.Count() == _fixture.Db.Roles.Count());
        }

        [Fact]
        public async Task RemoveAsync()
        {
            var nbRolesAtBeginning = _fixture.Db.Roles.Count();
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);
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
        public async Task RemoveNullAsync()
        {
            var repository = new RoleRepository(_fixture.Db, _fixture.RoleManager);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.RemoveAsync(null));
        }

        // --------

        [Fact]
        public void AddRole()
        {
            // Arrange
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);
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
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);

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
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);

            // Act & Assert
            Assert.True(_fixture.Db.Roles.Count() == roleRepository.CountAll());
        }

        [Fact]
        public async Task CountAllAsync()
        {
            // Arrange
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);

            // Act & Assert
            Assert.True(_fixture.Db.Roles.Count() == await roleRepository.CountAllAsync());
        }


        [Fact]
        public void GetAll()
        {
            // Arrange
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);

            // Act
            var result = roleRepository.GetAll();

            // Assert
            Assert.True(result.Count() == _fixture.Db.Roles.Count());
        }

        [Fact]
        public async Task GetAsyncSpecificationBasic()
        {
            // Arrange
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);
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
        public async Task GetAsyncWithTwoSpecifications()
        {
            // Arrange
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);
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
            Assert.True(result.Count == 1 && result.First().Name == testRole5.Name);
        }

        [Fact]
        public async Task GetAsyncWithTwoSortsAndTwoSpecificationsAndPagination()
        {
            // Arrange
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);
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
            Assert.True(result.Count == 4 && result.First().Name == role4.Entity.Name);
        }

        [Fact]
        public async Task GetAsyncWithNoArgument()
        {
            // Arrange
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);

            // Act & Assert
            Assert.True((await roleRepository.GetAsync()).ToList().Count == _fixture.Db.Roles.Count());
        }

        [Fact]
        public async Task GetAsyncWithAllArguments()
        {
            // Arrange
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);
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
            Assert.True(result.Count == 2 && result.First().Name == testRole2.Name);
        }

        [Fact]
        public async Task GetAsyncWithPagination()
        {
            // Arrange
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);
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
            Assert.True(result.Count == 3);
            Assert.Contains(result, x => x.Id == testRole3.Id &&
                                         x.Name == testRole3.Name);
            Assert.Contains(result, x => x.Id == testRole4.Id &&
                                         x.Name == testRole4.Name);
            Assert.Contains(result, x => x.Id == testRole5.Id &&
                                         x.Name == testRole5.Name);
        }

        [Fact]
        public async Task GetAsyncWithPaginationTakeOutOfRange()
        {
            // Arrange
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);
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
            Assert.True(result.Count == 4);
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
        public async Task GetAsyncWithPaginationTakeNegative()
        {
            // Arrange
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);
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
        public async Task GetAsyncWithPaginationSkipNegative()
        {
            // Arrange
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);
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
            Assert.True(result.Count == 3);
            Assert.Contains(result, x => x.Id == testRole.Id &&
                                         x.Name == testRole.Name);
            Assert.Contains(result, x => x.Id == testRole2.Id &&
                                         x.Name == testRole2.Name);
            Assert.Contains(result, x => x.Id == testRole3.Id &&
                                         x.Name == testRole3.Name);
        }

        [Fact]
        public async Task GetAsyncWithPaginationSkipOutOfRange()
        {
            // Arrange
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);
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
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);
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
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);
            var testRole = new Role()
            {
                Name = "GetRole"
            };
            roleRepository.Add(testRole);
            _fixture.UnitOfWork.Save();

            // Act
            var result = roleRepository.Get(testRole.Id);

            // Act & Assert
            Assert.True(result == _fixture.Db.Roles.Find(testRole.Id));
        }

        [Fact]
        public void GetCategoryOutOfRange()
        {
            // Arrange
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);

            // Act & Assert
            Assert.Throws<IndexOutOfRangeException>(() => roleRepository.Get(100));
        }

        [Fact]
        public void RemoveNull()
        {
            // Arrange
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => roleRepository.Remove(null));
        }

        [Fact]
        public async Task RemoveRangeAsyncNull()
        {
            // Arrange
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await roleRepository.RemoveRangeAsync(null));
        }

        [Fact]
        public void RemoveRangeNull()
        {
            // Arrange
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => roleRepository.RemoveRange(null));
        }

        [Fact]
        public void RemoveRange()
        {
            // Arrange
            var nbCategoriesAtBeginning = _fixture.Db.Roles.Count();
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);
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
        public async Task RemoveRangeAsync()
        {
            // Arrange
            var nbCategoriesAtBeginning = _fixture.Db.Roles.Count();
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);
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
        public async Task NameAlreadyExistsFalse()
        {
            // Arrange
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);

            // Act & Assert
            Assert.True(!await roleRepository.NameAlreadyExists("NameAlreadyExistsFalse"));
        }

        [Fact]
        public async Task NameAlreadyExistsNull()
        {
            // Arrange
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);

            // Act & Assert
            Assert.True(!await roleRepository.NameAlreadyExists(null));
        }

        [Fact]
        public async Task NameAlreadyExistsTrue()
        {
            // Arrange
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);
            var testRole = new Role()
            {
                Name = "NameAlreadyExistsTrue",
            };
            await roleRepository.AddAsync(testRole);
            await _fixture.Db.SaveChangesAsync();

            // Act & Assert
            Assert.True(await roleRepository.NameAlreadyExists(testRole.Name));
        }

        [Fact]
        public async Task AddPermission()
        {
            // Arrange
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);
            var testRole = new RoleBuilder(roleRepository, _fixture.UnitOfWork).Build();
            var permission = new Permission()
            {
                PermissionAction = PermissionAction.CanCreate, PermissionRange = PermissionRange.Own,
                PermissionTarget = PermissionTarget.Comment
            };

            // Act
            await roleRepository.AddPermissionAsync(testRole.Id, permission);
            await _fixture.Db.SaveChangesAsync();

            // Assert
            var roleClaims = _fixture.Db.RoleClaims;
            Assert.Contains(roleClaims, x => x.ClaimValue ==  JsonSerializer.Serialize(permission));
        }

        [Fact]
        public async Task AddPermissionWithNullPermission()
        {
            // Arrange
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);
            var testRole = new RoleBuilder(roleRepository, _fixture.UnitOfWork).Build();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await roleRepository.AddPermissionAsync(testRole.Id, null));
        }

        [Fact]
        public async Task AddPermissionWithNonExistentRole()
        {
            // Arrange
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);
            var permission = new Permission()
            {
                PermissionAction = PermissionAction.CanCreate,
                PermissionRange = PermissionRange.Own,
                PermissionTarget = PermissionTarget.Comment
            };

            // Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await roleRepository.AddPermissionAsync(99991, permission));
        }

        [Fact]
        public async Task AddPermissionThatAlreadyExistsInARole()
        {
            // Arrange
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);
            var testRole = new RoleBuilder(roleRepository, _fixture.UnitOfWork).Build();
            var permission = new Permission()
            {
                PermissionAction = PermissionAction.CanCreate,
                PermissionRange = PermissionRange.Own,
                PermissionTarget = PermissionTarget.Comment
            };
            await roleRepository.AddPermissionAsync(testRole.Id, permission);
            await _fixture.Db.SaveChangesAsync();

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await roleRepository.AddPermissionAsync(testRole.Id, permission));
        }

        [Fact]
        public async Task GetPermissionsAsync()
        {
            // Arrange
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);
            var testRole = new RoleBuilder(roleRepository, _fixture.UnitOfWork).Build();
            var commentPermission = new Permission()
            {
                PermissionAction = PermissionAction.CanCreate,
                PermissionRange = PermissionRange.Own,
                PermissionTarget = PermissionTarget.Comment
            };
            var postPermission = new Permission()
            {
                PermissionAction = PermissionAction.CanRead,
                PermissionRange = PermissionRange.Own,
                PermissionTarget = PermissionTarget.Post
            };
            await roleRepository.AddPermissionAsync(testRole.Id, commentPermission);
            await roleRepository.AddPermissionAsync(testRole.Id, postPermission);
            await _fixture.Db.SaveChangesAsync();

            // Act
            var permissions = (await roleRepository.GetPermissionsAsync(testRole.Id)).ToList();

            // Assert
            Assert.NotNull(permissions);
            Assert.True(permissions.Count == 2);
            Assert.Contains(permissions, x => x.Equals(commentPermission));
            Assert.Contains(permissions, x => x.Equals(postPermission));
        }

        [Fact]
        public async Task GetPermissionsAsyncEmpty()
        {
            // Arrange
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);
            var testRole = new RoleBuilder(roleRepository, _fixture.UnitOfWork).Build();

            // Act
            var permissions = (await roleRepository.GetPermissionsAsync(testRole.Id)).ToList();

            // Assert
            Assert.NotNull(permissions);
            Assert.Empty(permissions);
        }

        [Fact]
        public async Task GetPermissionsWithNonExistentRole()
        {
            // Arrange
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);

            // Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await roleRepository.GetPermissionsAsync(99991));
        }

        [Fact]
        public async Task RemovePermissionAsync()
        {
            // Arrange
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);
            var testRole = new RoleBuilder(roleRepository, _fixture.UnitOfWork).Build();
            var commentPermission = new Permission()
            {
                PermissionAction = PermissionAction.CanCreate,
                PermissionRange = PermissionRange.Own,
                PermissionTarget = PermissionTarget.Comment
            };
            var postPermission = new Permission()
            {
                PermissionAction = PermissionAction.CanRead,
                PermissionRange = PermissionRange.Own,
                PermissionTarget = PermissionTarget.Post
            };
            var likePermission = new Permission()
            {
                PermissionAction = PermissionAction.CanRead,
                PermissionRange = PermissionRange.All,
                PermissionTarget = PermissionTarget.Like
            };
            await roleRepository.AddPermissionAsync(testRole.Id, commentPermission);
            await roleRepository.AddPermissionAsync(testRole.Id, postPermission);
            await roleRepository.AddPermissionAsync(testRole.Id, likePermission);
            await _fixture.Db.SaveChangesAsync();

            // Act
            await roleRepository.RemovePermissionAsync(testRole.Id, likePermission);

            // Assert
            var permissions = (await roleRepository.GetPermissionsAsync(testRole.Id)).ToList();
            Assert.NotNull(permissions);
            Assert.True(permissions.Count == 2);
            Assert.Contains(permissions, x => x.Equals(commentPermission));
            Assert.Contains(permissions, x => x.Equals(postPermission));
            Assert.DoesNotContain(permissions, x => x.Equals(likePermission));
        }

        [Fact]
        public async Task RemovePermissionAsyncWithNullPermission()
        {
            // Arrange
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);
            var testRole = new RoleBuilder(roleRepository, _fixture.UnitOfWork).Build();
            await _fixture.Db.SaveChangesAsync();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await roleRepository.RemovePermissionAsync(testRole.Id, null));
        }

        [Fact]
        public async Task RemovePermissionAsyncWithNonExistentRole()
        {
            // Arrange
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);
            var commentPermission = new Permission()
            {
                PermissionAction = PermissionAction.CanCreate,
                PermissionRange = PermissionRange.Own,
                PermissionTarget = PermissionTarget.Comment
            };

            // Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await roleRepository.RemovePermissionAsync(99991, commentPermission));
        }

        [Fact]
        public async Task GetRolesFromUser()
        {
            // Arrange
            var roleRepository = new RoleRepository(_fixture.Db, _fixture.RoleManager);
            var userRepository = new DBAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var role1 = new RoleBuilder(roleRepository, _fixture.UnitOfWork).Build();
            var role2 = new RoleBuilder(roleRepository, _fixture.UnitOfWork).Build();
            var user = new UserBuilder(userRepository, _fixture.UnitOfWork).WithRole(role1).WithRole(role2).Build();

            // Act
            var roles = (await roleRepository.GetRolesFromUser(user.Id)).ToList();

            // Assert
            Assert.NotNull(roles);
            Assert.Contains(roles, x => x.Id == role1.Id && x.Name == role1.Name);
            Assert.Contains(roles, x => x.Id == role2.Id && x.Name == role2.Name);
        }
    }
}
