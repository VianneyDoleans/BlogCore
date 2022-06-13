using System;
using System.Linq.Expressions;
using DBAccess.Contracts;
using DBAccess.Data;

namespace DBAccess.Specifications.FilterSpecifications.Filters
{
    public class AuthorUsernameContainsSpecification<TEntity> : FilterSpecification<TEntity> where TEntity : class, IPoco, IHasAuthor
    {
        private readonly string _username;

        public AuthorUsernameContainsSpecification(string username)
        {
            _username = username;
        }

        protected override Expression<Func<TEntity, bool>> SpecificationExpression => p => p.Author.UserName.Contains(_username);
    }
}
