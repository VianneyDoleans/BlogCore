using System;
using System.Linq;
using System.Linq.Expressions;
using DBAccess.Contracts;
using DBAccess.Data;

namespace DBAccess.Specifications.FilterSpecifications.Filters
{
    public class TagSpecification<TEntity> : FilterSpecification<TEntity> where TEntity : class, IPoco, IHasPostTag
    {
        private readonly string _tag;
        
        public TagSpecification(string tag)
        {
            _tag = tag;
        }

        protected override Expression<Func<TEntity, bool>> SpecificationExpression => p => p.PostTags.Any(pt => pt.Tag.Name == _tag);
    }
}
