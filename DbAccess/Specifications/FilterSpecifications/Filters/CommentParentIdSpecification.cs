using System;
using System.Linq.Expressions;
using DBAccess.Data.POCO.Interface;

namespace DBAccess.Specifications.FilterSpecifications.Filters
{
    public class CommentParentIdSpecification<TEntity> : FilterSpecification<TEntity> where TEntity : class, IPoco, IHasCommentParent
    {
        private readonly int _id;

        public CommentParentIdSpecification(int id)
        {
            _id = id;
        }

        protected override Expression<Func<TEntity, bool>> SpecificationExpression => p => p.CommentParent != null && p.CommentParent.Id == _id;
    }
}
