using System;
using System.Linq.Expressions;
using DBAccess.Data;
using DBAccess.Specifications.FilterSpecifications.Filters;
using Xunit;

namespace DBAccess.Tests.SpecificationTests.Filters
{
    public class UserUsernameContainsSpecificationTests
    {
        [Theory]
        [InlineData("Jackson")]
        [InlineData("Jacksonknew")]
        public void CanCheckValidMatchItem(string usernameSpecification)
        {
            // Arrange
            var specification = new UserUsernameContainsSpecification<Like>(usernameSpecification);
            var function = ((Expression<Func<Like, bool>>)specification).Compile();

            // Act
            var matchSpecification = function.Invoke(new Like() { User = new User { UserName = "Jacksonknew" } });

            // Assert
            Assert.True(matchSpecification);
        }

        [Theory]
        [InlineData("Jacksonknewhe")]
        [InlineData("jacksonknew")]
        [InlineData("JacksonKnew")]
        public void CanCheckInvalidMatchItem(string usernameSpecification)
        {
            // Arrange
            var specification = new UserUsernameContainsSpecification<Like>(usernameSpecification);
            var function = ((Expression<Func<Like, bool>>)specification).Compile();

            // Act
            var matchSpecification = function.Invoke(new Like() { User = new User { UserName = "Jacksonknew" } });

            // Assert
            Assert.False(matchSpecification);
        }
    }
}
