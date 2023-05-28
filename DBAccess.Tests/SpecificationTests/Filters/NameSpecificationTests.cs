using DBAccess.Data;
using DBAccess.Specifications.FilterSpecifications.Filters;
using System;
using System.Linq.Expressions;
using Xunit;

namespace DBAccess.Tests.SpecificationTests.Filters
{
    public class NameSpecificationTests
    {
        [Theory]
        [InlineData("water")]
        public void CanCheckValidMatchItem(string name)
        {
            // Arrange
            var specification = new NameSpecification<Post>("water");
            var function = ((Expression<Func<Post, bool>>)specification).Compile();

            // Act
            var matchSpecification = function.Invoke(new Post { Name = name });

            // Assert
            Assert.True(matchSpecification);
        }

        [Theory]
        [InlineData("lava")]
        [InlineData("A glass of water")]
        public void CanCheckInvalidMatchItem(string name)
        {
            // Arrange
            var specification = new NameSpecification<Post>("water");
            var function = ((Expression<Func<Post, bool>>)specification).Compile();

            // Act
            var matchSpecification = function.Invoke(new Post { Name = name });

            // Assert
            Assert.False(matchSpecification);
        }
    }
}
