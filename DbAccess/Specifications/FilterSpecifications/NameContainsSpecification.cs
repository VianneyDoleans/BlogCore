using System;
using System.Linq.Expressions;
using DbAccess.Data.POCO.Interface;

namespace DbAccess.Specifications.FilterSpecifications
{
    public class NameContainsSpecification<TEntity> : FilterSpecification<TEntity> where TEntity : class, IPoco, IHasName
    {
        private readonly string _name;

        public NameContainsSpecification(string name)
        {
            _name = name;
        }

        protected override Expression<Func<TEntity, bool>> SpecificationExpression => p => p.Name.Contains(_name);
    }
}
