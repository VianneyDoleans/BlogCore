using System;
using System.Linq.Expressions;
using DBAccess.Data;
using DBAccess.Specifications.FilterSpecifications.Filters;
using Xunit;

namespace DBAccess.Tests.SpecificationTests.Filters
{
    public class PublishedAfterDateSpecificationTests
    {
        [Theory]
        [InlineData("2022-01-01 0:00:00Z")]
        [InlineData("2023-05-27 9:15:09Z")]
        public void CanCheckValidMatchItem(DateTimeOffset publishedDateSpecification)
        {
            // Arrange
            var specification = new PublishedAfterDateSpecification<Post>(publishedDateSpecification);
            var function = ((Expression<Func<Post, bool>>)specification).Compile();
            var publishDateOfPost = new DateTimeOffset(new DateTime(2023, 05, 27, 9, 15, 10, DateTimeKind.Utc));

            // Act
            var matchSpecification = function.Invoke(new Post { PublishedAt = publishDateOfPost });

            // Assert
            Assert.True(matchSpecification);
        }

        [Theory]
        [InlineData("2024-01-01 0:00:00Z")]
        [InlineData("2023-05-27 9:15:10Z")]
        [InlineData("2023-05-27 9:15:11Z")]
        public void CanCheckInvalidMatchItem(DateTimeOffset publishedDateSpecification)
        {
            // Arrange
            var specification = new PublishedAfterDateSpecification<Post>(publishedDateSpecification);
            var function = ((Expression<Func<Post, bool>>)specification).Compile();
            var publishDateOfPost = new DateTimeOffset(new DateTime(2023, 05, 27, 9, 15, 10, DateTimeKind.Utc));

            // Act
            var matchSpecification = function.Invoke(new Post() { PublishedAt = publishDateOfPost });

            // Assert
            Assert.False(matchSpecification);
        }
    }
}
