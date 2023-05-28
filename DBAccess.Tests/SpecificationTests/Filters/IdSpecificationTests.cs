using DBAccess.Specifications.FilterSpecifications.Filters;
using System;
using System.Linq.Expressions;
using DBAccess.Data;
using Xunit;

namespace DBAccess.Tests.SpecificationTests.Filters
{
    public class IdSpecificationTests
    {
        [Fact]
        public void CanCheckValidMatchItem()
        {
            // Arrange
            var specification = new IdSpecification<User>(1);
            var function = ((Expression<Func<User, bool>>)specification).Compile();

            // Act
            var matchSpecification = function.Invoke(new User { Id = 1 });

            // Assert
            Assert.True(matchSpecification);
        }

        [Fact]
        public void CanCheckInvalidMatchItem()
        {
            // Arrange
            var specification = new IdSpecification<User>(2);
            var function = ((Expression<Func<User, bool>>)specification).Compile();

            // Act
            var matchSpecification = function.Invoke(new User { Id = 1 });

            // Assert
            Assert.False(matchSpecification);
        }
    }
}
