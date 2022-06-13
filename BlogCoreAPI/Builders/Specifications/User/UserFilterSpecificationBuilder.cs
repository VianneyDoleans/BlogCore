using System;
using DBAccess.Data;
using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.FilterSpecifications.Filters;

namespace BlogCoreAPI.Builders.Specifications.User
{
    /// <summary>
    /// Class used to generate <see cref="FilterSpecification{TEntity}"/> for <see cref="User"/>.
    /// </summary>
    public class UserFilterSpecificationBuilder
    {
        private readonly string _username;
        private readonly DateTime? _lastLoginBefore;
        private readonly DateTime? _registerBefore;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserFilterSpecificationBuilder"/> class.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="lastLoginBefore"></param>
        /// <param name="registerBefore"></param>
        public UserFilterSpecificationBuilder(string username, DateTime? lastLoginBefore, DateTime? registerBefore)
        {
            _username = username;
            _lastLoginBefore = lastLoginBefore;
            _registerBefore = registerBefore;
        }

        /// <summary>
        /// Get filter specification of <see cref="User"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public FilterSpecification<DBAccess.Data.User> Build()
        {

            FilterSpecification<DBAccess.Data.User> filter = null;

            if (_lastLoginBefore != null)
                filter = new LastLoginBeforeDateSpecification<DBAccess.Data.User>(_lastLoginBefore.Value);
            if (_registerBefore != null)
            {
                filter = filter == null ?
                    new RegisterBeforeDateSpecification<DBAccess.Data.User>(_registerBefore.Value) 
                    : filter & new RegisterBeforeDateSpecification<DBAccess.Data.User>(_registerBefore.Value);
            }
            if (_username != null)
            {
                filter = filter == null ?
                    new UsernameContainsSpecification<DBAccess.Data.User>(_username) 
                    : filter & new UsernameContainsSpecification<DBAccess.Data.User>(_username);
            }

            return filter;
        }
    }
}
