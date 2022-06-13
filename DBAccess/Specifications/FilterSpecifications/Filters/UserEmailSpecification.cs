using System;
using System.Linq.Expressions;
using DBAccess.Contracts;
using DBAccess.Data;

namespace DBAccess.Specifications.FilterSpecifications.Filters
{
    public class UserEmailSpecification<TEntity> : FilterSpecification<TEntity> where TEntity : class, IPoco, IHasUser
    { 
        private readonly string _emailAddress;
            
        public UserEmailSpecification(string emailAddress) 
        { 
            _emailAddress = emailAddress;
        }
    
        protected override Expression<Func<TEntity, bool>> SpecificationExpression => p => p.User.Email == _emailAddress;
    }
}
