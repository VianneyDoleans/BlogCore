using DBAccess.Specifications.FilterSpecifications.Filters;
using System.Linq.Expressions;
using System;
using DBAccess.Data;
using Xunit;

namespace DBAccess.Tests.SpecificationTests.Filters
{
    public class NameContainsSpecificationTests
    {
        [Fact]
        public void CanCheckValidMatchItem()
        {
            // Arrange
            var specification = new NameContainsSpecification<Post>("water");
            var function = ((Expression<Func<Post, bool>>)specification).Compile();

            // Act
            var matchSpecification = function.Invoke(new Post { Name = "A glass of water" });

            // Assert
            Assert.True(matchSpecification);
        }

        [Fact]
        public void CanCheckInvalidMatchItem()
        {
            // Arrange
            var specification = new NameContainsSpecification<Post>("water");
            var function = ((Expression<Func<Post, bool>>)specification).Compile();

            // Act
            var matchSpecification = function.Invoke(new Post { Name = "A glass of lava" });

            // Assert
            Assert.False(matchSpecification);
        }
    }
}
