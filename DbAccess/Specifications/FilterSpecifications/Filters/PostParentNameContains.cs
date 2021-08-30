using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using DbAccess.Data.POCO.Interface;

namespace DbAccess.Specifications.FilterSpecifications.Filters
{
    public class PostParentNameContains<TEntity> : FilterSpecification<TEntity> where TEntity : class, IPoco, IHasPostParent
    {
        private readonly string _name;

        public PostParentNameContains(string name)
        {
            _name = name;
        }

        protected override Expression<Func<TEntity, bool>> SpecificationExpression => p => p.PostParent.Name == _name;
    }
}
