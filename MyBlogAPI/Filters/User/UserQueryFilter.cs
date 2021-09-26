using System;
using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.FilterSpecifications.Filters;

namespace MyBlogAPI.Filters.User
{
    public class UserQueryFilter
    {
        private readonly string _username;
        private readonly DateTime? _lastLoginBefore;
        private readonly DateTime? _registerBefore;

        public UserQueryFilter(string username, DateTime? lastLoginBefore, DateTime? registerBefore)
        {
            _username = username;
            _lastLoginBefore = lastLoginBefore;
            _registerBefore = registerBefore;
        }

        public FilterSpecification<DbAccess.Data.POCO.User> GetFilterSpecification()
        {

            FilterSpecification<DbAccess.Data.POCO.User> filter = null;

            if (_lastLoginBefore != null)
                filter = new LastLoginBeforeDateSpecification<DbAccess.Data.POCO.User>(_lastLoginBefore.Value);
            if (_registerBefore != null)
            {
                if (_registerBefore == null)
                    filter = new RegisterBeforeDateSpecification<DbAccess.Data.POCO.User>(_registerBefore.Value);
                else
                    filter &= new RegisterBeforeDateSpecification<DbAccess.Data.POCO.User>(_registerBefore.Value);
            }
            if (_username != null)
            {
                if (filter == null)
                    filter = new UsernameContainsSpecification<DbAccess.Data.POCO.User>(_username);
                else
                    filter &= new UsernameContainsSpecification<DbAccess.Data.POCO.User>(_username);
            }

            return filter;
        }
    }
}
