using DBAccess.Specifications.FilterSpecifications.Filters;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using DBAccess.Data;
using Xunit;

namespace DBAccess.Tests.SpecificationTests.Filters
{
    public class MaximumLikeCountSpecificationTests
    {
        [Fact]
        public void CanCheckValidMatchItem()
        {
            // Arrange
            var specification = new MaximumLikeCountSpecification<Post>(3);
            var function = ((Expression<Func<Post, bool>>)specification).Compile();

            // Act
            var matchSpecification = function.Invoke(new Post { Likes = new List<Like>() {new (), new()} });

            // Assert
            Assert.True(matchSpecification);
        }

        [Fact]
        public void SameNumberOfItemsIsValidMatch()
        {
            // Arrange
            var specification = new MaximumLikeCountSpecification<Post>(3);
            var function = ((Expression<Func<Post, bool>>)specification).Compile();

            // Act
            var matchSpecification = function.Invoke(new Post { Likes = new List<Like>() { new(), new(), new() } });

            // Assert
            Assert.True(matchSpecification);
        }

        [Fact]
        public void CanCheckInvalidMatchItem()
        {
            // Arrange
            var specification = new MaximumLikeCountSpecification<Post>(3);
            var function = ((Expression<Func<Post, bool>>)specification).Compile();

            // Act
            var matchSpecification = function.Invoke(new Post { Likes = new List<Like>() { new(), new(), new(), new() } });

            // Assert
            Assert.False(matchSpecification);
        }
    }
}
