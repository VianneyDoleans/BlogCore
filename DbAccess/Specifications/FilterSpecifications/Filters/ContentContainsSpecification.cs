using System;
using System.Linq.Expressions;
using DbAccess.Data.POCO.Interface;

namespace DbAccess.Specifications.FilterSpecifications.Filters
{
    public class ContentContainsSpecification<TEntity> : FilterSpecification<TEntity> where TEntity : class, IPoco, IHasContent
    {
        private readonly string _content;

        public ContentContainsSpecification(string content)
        {
            _content = content;
        }

        protected override Expression<Func<TEntity, bool>> SpecificationExpression => p => p.Content.Contains(_content);
    }
}
