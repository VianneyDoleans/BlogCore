using System;
using System.Linq.Expressions;
using DBAccess.Data.POCO.Interface;

namespace DBAccess.Specifications.FilterSpecifications.Filters
{
    public class UserUsernameContainsSpecification<TEntity> : FilterSpecification<TEntity> where TEntity : class, IPoco, IHasUser
    {
        private readonly string _username;

        public UserUsernameContainsSpecification(string username)
        {
            _username = username;
        }

        protected override Expression<Func<TEntity, bool>> SpecificationExpression => p => p.User.UserName.Contains(_username);
    }
}
