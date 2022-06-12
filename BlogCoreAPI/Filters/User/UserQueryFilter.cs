using System;
using DBAccess.Data.POCO;
using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.FilterSpecifications.Filters;

namespace BlogCoreAPI.Filters.User
{
    /// <summary>
    /// Class used to generate <see cref="FilterSpecification{TEntity}"/> for <see cref="User"/>.
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
        /// Get filter specification of <see cref="User"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public FilterSpecification<DBAccess.Data.POCO.User> GetFilterSpecification()
        {

            FilterSpecification<DBAccess.Data.POCO.User> filter = null;

            if (_lastLoginBefore != null)
                filter = new LastLoginBeforeDateSpecification<DBAccess.Data.POCO.User>(_lastLoginBefore.Value);
            if (_registerBefore != null)
            {
                filter = filter == null ?
                    new RegisterBeforeDateSpecification<DBAccess.Data.POCO.User>(_registerBefore.Value) 
                    : filter & new RegisterBeforeDateSpecification<DBAccess.Data.POCO.User>(_registerBefore.Value);
            }
            if (_username != null)
            {
                filter = filter == null ?
                    new UsernameContainsSpecification<DBAccess.Data.POCO.User>(_username) 
                    : filter & new UsernameContainsSpecification<DBAccess.Data.POCO.User>(_username);
            }

            return filter;
        }
    }
}
