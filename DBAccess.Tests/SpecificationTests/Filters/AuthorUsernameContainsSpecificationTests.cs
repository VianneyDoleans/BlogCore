using System.Linq.Expressions;
using System;
using DBAccess.Data;
using DBAccess.Specifications.FilterSpecifications.Filters;
using Xunit;

namespace DBAccess.Tests.SpecificationTests.Filters
{
    public class AuthorUsernameContainsSpecificationTests
    {
        [Fact]
        public void CanCheckValidMatchItem()
        {
            // Arrange
            var specification = new AuthorUsernameContainsSpecification<Post>("test");
            var function = ((Expression<Func<Post, bool>>)specification).Compile();

            // Act
            var matchSpecification = function.Invoke(new Post() { Author = new User() { UserName = "a_valid_test_username" } });

            // Assert
            Assert.True(matchSpecification);
        }

        [Fact]
        public void CanCheckInvalidMatchItem()
        {
            // Arrange
            var specification = new AuthorUsernameContainsSpecification<Post>("invalid_name");
            var function = ((Expression<Func<Post, bool>>)specification).Compile();

            // Act
            var matchSpecification = function.Invoke(new Post() { Author = new User() { UserName = "an_invalid_test_username" } });

            // Assert
            Assert.False(matchSpecification);
        }
    }
}
