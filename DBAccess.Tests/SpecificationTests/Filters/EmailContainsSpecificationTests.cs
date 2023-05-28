using DBAccess.Data;
using DBAccess.Specifications.FilterSpecifications.Filters;
using System;
using System.Linq.Expressions;
using Xunit;

namespace DBAccess.Tests.SpecificationTests.Filters
{
    public class EmailContainsSpecificationTests
    {
        [Fact]
        public void CanCheckValidMatchItem()
        {
            // Arrange
            var specification = new EmailContainsSpecification<User>("water");
            var function = ((Expression<Func<User, bool>>)specification).Compile();

            // Act
            var matchSpecification = function.Invoke(new User { Email = "water.test@email.com" });

            // Assert
            Assert.True(matchSpecification);
        }

        [Fact]
        public void CanCheckInvalidMatchItem()
        {
            // Arrange
            var specification = new EmailContainsSpecification<User>("water");
            var function = ((Expression<Func<User, bool>>)specification).Compile();

            // Act
            var matchSpecification = function.Invoke(new User { Email = "lava.test@email.com" });

            // Assert
            Assert.False(matchSpecification);
        }
    }
}
