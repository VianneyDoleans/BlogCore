using DBAccess.Data;
using DBAccess.Specifications.FilterSpecifications.Filters;
using System;
using System.Linq.Expressions;
using Xunit;

namespace DBAccess.Tests.SpecificationTests.Filters
{
    public class ContentContainsSpecificationTests
    {
        [Fact]
        public void CanCheckValidMatchItem()
        {
            // Arrange
            var specification = new ContentContainsSpecification<Post>("water");
            var function = ((Expression<Func<Post, bool>>)specification).Compile();

            // Act
            var matchSpecification = function.Invoke(new Post { Content = "A glass of water" });

            // Assert
            Assert.True(matchSpecification);
        }

        [Fact]
        public void CanCheckInvalidMatchItem()
        {
            // Arrange
            var specification = new ContentContainsSpecification<Post>("water");
            var function = ((Expression<Func<Post, bool>>)specification).Compile();

            // Act
            var matchSpecification = function.Invoke(new Post { Content = "A glass of lava" });

            // Assert
            Assert.False(matchSpecification);
        }
    }
}
