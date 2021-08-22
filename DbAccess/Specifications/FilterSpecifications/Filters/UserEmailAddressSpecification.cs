using System;
using System.Linq.Expressions;
using DbAccess.Data.POCO.Interface;

namespace DbAccess.Specifications.FilterSpecifications.Filters
{
    public class UserEmailAddressSpecification<TEntity> : FilterSpecification<TEntity> where TEntity : class, IPoco, IHasUser
    { 
        private readonly string _emailAddress;
            
        public UserEmailAddressSpecification(string emailAddress) 
        { 
            _emailAddress = emailAddress;
        }
    
        protected override Expression<Func<TEntity, bool>> SpecificationExpression => p => p.User.EmailAddress == _emailAddress;
    }
}
