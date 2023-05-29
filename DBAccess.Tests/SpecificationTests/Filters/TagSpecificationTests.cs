using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DBAccess.Data;
using DBAccess.Data.JoiningEntity;
using DBAccess.Specifications.FilterSpecifications.Filters;
using Xunit;

namespace DBAccess.Tests.SpecificationTests.Filters
{
    public class TagSpecificationTests
    {
        [Theory]
        [InlineData("water")]
        public void CanCheckValidMatchItem(string name)
        {
            // Arrange
            var specification = new TagSpecification<Post>("water");
            var function = ((Expression<Func<Post, bool>>)specification).Compile();

            // Act
            var matchSpecification = function.Invoke(
                new Post { PostTags = new List<PostTag>() { new() { Tag = new Tag() { Name = name } } } });

            // Assert
            Assert.True(matchSpecification);
        }

        [Theory]
        [InlineData("lava")]
        [InlineData("A glass of water")]
        public void CanCheckInvalidMatchItem(string name)
        {
            // Arrange
            var specification = new TagSpecification<Post>("water");
            var function = ((Expression<Func<Post, bool>>)specification).Compile();

            // Act
            var matchSpecification = function.Invoke(
                new Post { PostTags = new List<PostTag>() { new() { Tag = new Tag() { Name = name } } } });

            // Assert
            Assert.False(matchSpecification);
        }
    }
}
