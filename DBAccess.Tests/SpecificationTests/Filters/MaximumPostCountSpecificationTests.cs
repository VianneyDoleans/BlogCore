using DBAccess.Data;
using DBAccess.Specifications.FilterSpecifications.Filters;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using Xunit;

namespace DBAccess.Tests.SpecificationTests.Filters
{
    public class MaximumPostCountSpecificationTests
    {
        [Fact]
        public void CanCheckValidMatchItem()
        {
            // Arrange
            var specification = new MaximumPostCountSpecification<User>(3);
            var function = ((Expression<Func<User, bool>>)specification).Compile();

            // Act
            var matchSpecification = function.Invoke(new User { Posts = new List<Post>() { new(), new() } });

            // Assert
            Assert.True(matchSpecification);
        }

        [Fact]
        public void SameNumberOfItemsIsValidMatch()
        {
            // Arrange
            var specification = new MaximumPostCountSpecification<User>(3);
            var function = ((Expression<Func<User, bool>>)specification).Compile();

            // Act
            var matchSpecification = function.Invoke(new User { Posts = new List<Post>() { new(), new(), new() } });

            // Assert
            Assert.True(matchSpecification);
        }

        [Fact]
        public void CanCheckInvalidMatchItem()
        {
            // Arrange
            var specification = new MaximumPostCountSpecification<User>(3);
            var function = ((Expression<Func<User, bool>>)specification).Compile();

            // Act
            var matchSpecification = function.Invoke(new User { Posts = new List<Post>() { new(), new(), new(), new() } });

            // Assert
            Assert.False(matchSpecification);
        }
    }
}
