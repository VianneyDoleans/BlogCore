﻿using System;
using System.Linq.Expressions;
using DBAccess.Contracts;
using DBAccess.Data;

namespace DBAccess.Specifications.FilterSpecifications.Filters
{
    public class PostParentNameContainsSpecification<TEntity> : FilterSpecification<TEntity> where TEntity : class, IPoco, IHasPostParent
    {
        private readonly string _name;

        public PostParentNameContainsSpecification(string name)
        {
            _name = name;
        }

        protected override Expression<Func<TEntity, bool>> SpecificationExpression => p => p.PostParent != null && p.PostParent.Name.Contains(_name);
    }
}
