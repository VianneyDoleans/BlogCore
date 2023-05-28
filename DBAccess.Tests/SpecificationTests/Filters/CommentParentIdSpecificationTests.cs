using DBAccess.Data;
using DBAccess.Specifications.FilterSpecifications.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DBAccess.Tests.SpecificationTests.Filters
{
    public class CommentParentIdSpecificationTests
    {
        [Fact]
        public void CanCheckValidMatchItem()
        {
            // Arrange
            var specification = new CommentParentIdSpecification<Comment>(1);
            var function = ((Expression<Func<Comment, bool>>)specification).Compile();

            // Act
            var matchSpecification = function.Invoke(new Comment() { CommentParent = new Comment { Id = 1 } });

            // Assert
            Assert.True(matchSpecification);
        }

        [Fact]
        public void CanCheckInvalidMatchItem()
        {
            // Arrange
            var specification = new CommentParentIdSpecification<Comment>(2);
            var function = ((Expression<Func<Comment, bool>>)specification).Compile();

            // Act
            var matchSpecification = function.Invoke(new Comment() { CommentParent = new Comment { Id = 1 } });

            // Assert
            Assert.False(matchSpecification);
        }
    }
}
