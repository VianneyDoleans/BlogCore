﻿using System;
using System.Linq.Expressions;
using DbAccess.Data.POCO.Interface;

namespace DbAccess.Specifications.FilterSpecifications.Filters
{
    public class MaximumPostNumberSpecification<TEntity> : FilterSpecification<TEntity> where TEntity : class, IPoco, IHasPosts
    {
        private readonly int _number;

        public MaximumPostNumberSpecification(int number)
        {
            _number = number;
        }

        protected override Expression<Func<TEntity, bool>> SpecificationExpression => p => p.Posts.Count <= _number;
    }
}