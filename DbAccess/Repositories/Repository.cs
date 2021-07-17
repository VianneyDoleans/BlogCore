using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DbAccess.DataContext;
using DbAccess.Specifications;
using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.SortSpecification;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly MyBlogContext Context;

        public Repository(MyBlogContext context)
        {
            Context = context;
        }

        public virtual async Task<TEntity> GetAsync(int id)
        {
            var result = await Context.Set<TEntity>().FindAsync(id);
            if (result == null)
                throw new IndexOutOfRangeException("Element doesn't exists.");
            return result;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        private static Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> SortOrderBy(Sort<TEntity> sortElement,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> sort)
        {
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> result = null;

            if (sortElement.OrderBy != null &&
                sortElement.SortingDirection == SortingDirectionSpecification.Ascending)
                result = items => sort(items).ThenBy(sortElement.OrderBy.Order);

            if (sortElement.OrderBy != null &&
                sortElement.SortingDirection == SortingDirectionSpecification.Descending)
                result = items => sort(items).ThenByDescending(sortElement.OrderBy.Order);
            return result;
        }

        private static Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> SortThenBy(Sort<TEntity> sortElement)
        {
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> result = null;

            if (sortElement.OrderBy != null &&
                sortElement.SortingDirection == SortingDirectionSpecification.Ascending)
                result = items => items.OrderBy(sortElement.OrderBy.Order);

            if (sortElement.OrderBy != null &&
                sortElement.SortingDirection == SortingDirectionSpecification.Descending)
                result = items => items.OrderByDescending(sortElement.OrderBy.Order);
            return result;
        }

        protected static IQueryable<TEntity> SortQuery(SortSpecification<TEntity> sortSpecification, IQueryable<TEntity> query)
        {
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> sort = null;

            foreach (var sortElement in sortSpecification.SortElements)
            {
                sort = sort != null ? SortOrderBy(sortElement, sort) : SortThenBy(sortElement);
            }

            if (sort != null)
                query = sort(query);
            return query;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(FilterSpecification<TEntity> filterSpecification = null, 
            PagingSpecification pagingSpecification = null,
            SortSpecification<TEntity> sortSpecification = null)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();
            if (filterSpecification != null)
                query = query.Where(filterSpecification);

            if (sortSpecification != null)
                query = SortQuery(sortSpecification, query);

            if (pagingSpecification != null)
                query = query.Skip(pagingSpecification.Skip).Take(pagingSpecification.Take);

            return await query.ToListAsync();
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        { 
            return (await Context.Set<TEntity>().AddAsync(entity)).Entity;
        }

        public virtual Task RemoveAsync(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
            return Task.CompletedTask;
        }

        public virtual Task RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
            return Task.CompletedTask;
        }

        public Task<int> CountWhereAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).CountAsync();
        }

        public Task<int> CountAllAsync()
        {
            return Context.Set<TEntity>().CountAsync();
        }

        public virtual TEntity Get(int id)
        {
            var result = Context.Set<TEntity>().Find(id);
            if (result == null)
                throw new IndexOutOfRangeException("Element doesn't exist.");
            return result;
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().ToList();
        }

        public virtual TEntity Add(TEntity entity)
        {
            return Context.Set<TEntity>().Add(entity).Entity;
        }

        public virtual void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }

        public int CountWhere(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).Count();
        }

        public int CountAll()
        {
            return Context.Set<TEntity>().Count();
        }
    }
}
