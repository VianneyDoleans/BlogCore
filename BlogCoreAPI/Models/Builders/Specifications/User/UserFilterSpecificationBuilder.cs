using System;
using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.FilterSpecifications.Filters;

namespace BlogCoreAPI.Models.Builders.Specifications.User
{
    /// <summary>
    /// Class used to generate <see cref="FilterSpecification{TEntity}"/> for <see cref="User"/>.
    /// </summary>
    public class UserFilterSpecificationBuilder
    {
        private string _inUserName;
        private DateTime? _fromRegister;
        private DateTime? _toRegister;
        private DateTime? _fromLastLogin;
        private DateTime? _toLastLogin;

        public UserFilterSpecificationBuilder WithInUserName(string inUserName)
        {
            _inUserName = inUserName;
            return this;
        }

        public UserFilterSpecificationBuilder WithFromRegister(DateTime? fromRegister)
        {
            _fromRegister = fromRegister;
            return this;
        }

        public UserFilterSpecificationBuilder WithToRegister(DateTime? toRegister)
        {
            _toRegister = toRegister;
            return this;
        }

        public UserFilterSpecificationBuilder WithFromLastLogin(DateTime? fromLastLogin)
        {
            _fromLastLogin = fromLastLogin;
            return this;
        }

        public UserFilterSpecificationBuilder WithToLastLogin(DateTime? toLastLogin)
        {
            _toLastLogin = toLastLogin;
            return this;
        }

        /// <summary>
        /// Get filter specification of <see cref="User"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public FilterSpecification<DBAccess.Data.User> Build()
        {

            FilterSpecification<DBAccess.Data.User> filter = null;

            if (_fromLastLogin != null)
                filter = new LastLoginBeforeDateSpecification<DBAccess.Data.User>(_fromLastLogin.Value);
            if (_fromRegister!= null)
            {
                filter = filter == null ?
                    new RegisterBeforeDateSpecification<DBAccess.Data.User>(_fromRegister.Value) 
                    : filter & new RegisterBeforeDateSpecification<DBAccess.Data.User>(_fromRegister.Value);
            }
            if (_toLastLogin != null)
            {
                filter = filter == null
                    ? new LastLoginAfterDateSpecification<DBAccess.Data.User>(_toLastLogin.Value)
                    : filter & new LastLoginAfterDateSpecification<DBAccess.Data.User>(_toLastLogin.Value);
            }
            if (_toRegister != null)
            {
                filter = filter == null
                    ? new RegisterAfterDateSpecification<DBAccess.Data.User>(_toRegister.Value)
                    : filter & new RegisterAfterDateSpecification<DBAccess.Data.User>(_toRegister.Value);
            }
            if (_inUserName != null)
            {
                filter = filter == null ?
                    new UsernameContainsSpecification<DBAccess.Data.User>(_inUserName)
                    : filter & new UsernameContainsSpecification<DBAccess.Data.User>(_inUserName);
            }

            return filter;
        }
    }
}
