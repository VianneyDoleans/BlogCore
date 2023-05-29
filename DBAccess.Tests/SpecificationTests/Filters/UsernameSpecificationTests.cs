using System;
using System.Linq.Expressions;
using DBAccess.Data;
using DBAccess.Specifications.FilterSpecifications.Filters;
using Xunit;

namespace DBAccess.Tests.SpecificationTests.Filters
{
    public class UsernameSpecificationTests
    {
        [Theory]
        [InlineData("Jacksonknew")]
        public void CanCheckValidMatchItem(string usernameSpecification)
        {
            // Arrange
            var specification = new UsernameSpecification<User>(usernameSpecification);
            var function = ((Expression<Func<User, bool>>)specification).Compile();

            // Act
            var matchSpecification = function.Invoke(new User { UserName = "Jacksonknew" });

            // Assert
            Assert.True(matchSpecification);
        }

        [Theory]
        [InlineData("Jackson")]
        [InlineData("Jacksonknewhe")]
        [InlineData("jacksonknew")]
        [InlineData("JacksonKnew")]
        public void CanCheckInvalidMatchItem(string usernameSpecification)
        {
            // Arrange
            var specification = new UsernameSpecification<User>(usernameSpecification);
            var function = ((Expression<Func<User, bool>>)specification).Compile();

            // Act
            var matchSpecification = function.Invoke(new User() { UserName = "Jacksonknew" });

            // Assert
            Assert.False(matchSpecification);
        }
    }
}
