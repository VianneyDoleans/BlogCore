using DBAccess.Specifications.FilterSpecifications.Filters;
using System.Linq.Expressions;
using System;
using DBAccess.Data;
using Xunit;

namespace DBAccess.Tests.SpecificationTests.Filters
{
    public class LikeableTypeSpecificationTests
    {
        [Fact]
        public void CanCheckValidMatchItem()
        {
            // Arrange
            var specification = new LikeableTypeSpecification<Like>(LikeableType.Comment);
            var function = ((Expression<Func<Like, bool>>)specification).Compile();

            // Act
            var matchSpecification = function.Invoke(new Like { Comment = new Comment() { Id = 1 } });

            // Assert
            Assert.True(matchSpecification);
        }

        [Fact]
        public void CanCheckInvalidMatchItem()
        {
            // Arrange
            var specification = new LikeableTypeSpecification<Like>(LikeableType.Comment);
            var function = ((Expression<Func<Like, bool>>)specification).Compile();

            // Act
            var matchSpecification = function.Invoke(new Like { Post = new Post() { Id = 1 } });

            // Assert
            Assert.False(matchSpecification);
        }
    }
}
