using System;
using System.Linq.Expressions;
using DBAccess.Contracts;
using DBAccess.Data;

namespace DBAccess.Specifications.FilterSpecifications.Filters
{
    public class UsernameContainsSpecification<TEntity> : FilterSpecification<TEntity> where TEntity : class, IPoco, IHasUserName
    {
        private readonly string _username;

        public UsernameContainsSpecification(string username)
        {
            _username = username;
        }

        protected override Expression<Func<TEntity, bool>> SpecificationExpression => p => p.UserName.Contains(_username);
    }
}
