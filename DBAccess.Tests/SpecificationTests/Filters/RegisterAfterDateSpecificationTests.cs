using DBAccess.Data;
using DBAccess.Specifications.FilterSpecifications.Filters;
using System.Linq.Expressions;
using System;
using Xunit;

namespace DBAccess.Tests.SpecificationTests.Filters
{
    public class RegisterAfterDateSpecificationTests
    {
        [Theory]
        [InlineData("2022-01-01 0:00:00Z")]
        [InlineData("2023-05-27 9:15:09Z")]
        public void CanCheckValidMatchItem(DateTimeOffset registerDateSpecification)
        {
            // Arrange
            var specification = new RegisterAfterDateSpecification<User>(registerDateSpecification);
            var function = ((Expression<Func<User, bool>>)specification).Compile();
            var registerDateOfUser = new DateTimeOffset(new DateTime(2023, 05, 27, 9, 15, 10, DateTimeKind.Utc));

            // Act
            var matchSpecification = function.Invoke(new User { RegisteredAt = registerDateOfUser });

            // Assert
            Assert.True(matchSpecification);
        }

        [Theory]
        [InlineData("2024-01-01 0:00:00Z")]
        [InlineData("2023-05-27 9:15:10Z")]
        [InlineData("2023-05-27 9:15:11Z")]
        public void CanCheckInvalidMatchItem(DateTimeOffset registerDateSpecification)
        {
            // Arrange
            var specification = new RegisterAfterDateSpecification<User>(registerDateSpecification);
            var function = ((Expression<Func<User, bool>>)specification).Compile();
            var registerDateOfUser = new DateTimeOffset(new DateTime(2023, 05, 27, 9, 15, 10, DateTimeKind.Utc));

            // Act
            var matchSpecification = function.Invoke(new User() { RegisteredAt = registerDateOfUser });

            // Assert
            Assert.False(matchSpecification);
        }
    }
}
