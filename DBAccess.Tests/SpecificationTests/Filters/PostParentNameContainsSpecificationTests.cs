using DBAccess.Data;
using DBAccess.Specifications.FilterSpecifications.Filters;
using System;
using System.Linq.Expressions;
using Xunit;

namespace DBAccess.Tests.SpecificationTests.Filters
{
    public class PostParentNameContainsSpecificationTests
    {
        [Fact]
        public void CanCheckValidMatchItem()
        {
            // Arrange
            var specification = new PostParentNameContainsSpecification<Comment>("water");
            var function = ((Expression<Func<Comment, bool>>)specification).Compile();

            // Act
            var matchSpecification = function.Invoke(new Comment { PostParent = new Post() {Name = "A glass of water"} });

            // Assert
            Assert.True(matchSpecification);
        }

        [Fact]
        public void CanCheckInvalidMatchItem()
        {
            // Arrange
            var specification = new PostParentNameContainsSpecification<Comment>("water");
            var function = ((Expression<Func<Comment, bool>>)specification).Compile();

            // Act
            var matchSpecification = function.Invoke(new Comment { PostParent = new Post() { Name = "A glass of lava" } });

            // Assert
            Assert.False(matchSpecification);
        }
    }
}
