using System;
using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.FilterSpecifications.Filters;

namespace MyBlogAPI.Filters.User
{
    /// <summary>
    /// Class used to generate <see cref="FilterSpecification{TEntity}"/> for <see cref="DbAccess.Data.POCO.User"/>.
    /// </summary>
    public class UserQueryFilter
    {
        private readonly string _username;
        private readonly DateTime? _lastLoginBefore;
        private readonly DateTime? _registerBefore;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserQueryFilter"/> class.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="lastLoginBefore"></param>
        /// <param name="registerBefore"></param>
        public UserQueryFilter(string username, DateTime? lastLoginBefore, DateTime? registerBefore)
        {
            _username = username;
            _lastLoginBefore = lastLoginBefore;
            _registerBefore = registerBefore;
        }

        /// <summary>
        /// Get filter specification of <see cref="DbAccess.Data.POCO.User"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public FilterSpecification<DbAccess.Data.POCO.User> GetFilterSpecification()
        {

            FilterSpecification<DbAccess.Data.POCO.User> filter = null;

            if (_lastLoginBefore != null)
                filter = new LastLoginBeforeDateSpecification<DbAccess.Data.POCO.User>(_lastLoginBefore.Value);
            if (_registerBefore != null)
            {
                filter = filter == null ?
                    new RegisterBeforeDateSpecification<DbAccess.Data.POCO.User>(_registerBefore.Value) 
                    : filter & new RegisterBeforeDateSpecification<DbAccess.Data.POCO.User>(_registerBefore.Value);
            }
            if (_username != null)
            {
                filter = filter == null ?
                    new UsernameContainsSpecification<DbAccess.Data.POCO.User>(_username) 
                    : filter & new UsernameContainsSpecification<DbAccess.Data.POCO.User>(_username);
            }

            return filter;
        }
    }
}
