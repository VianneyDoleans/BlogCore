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
    /// <summary>
    /// Implementation of <see cref="IRepository{TEntity}"/> interface.
    /// It implements the available generics methods necessary to manipulate the Resources from the database (CRUD and more).
    /// It also defines some protected methods needed for Repositories class which inherited from <see cref="Repository{TEntity}"/>.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly MyBlogContext Context;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class.
        /// </summary>
        /// <param name="context"></param>
        public Repository(MyBlogContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Method used to generate <see cref="IQueryable{TEntity}"/> for a resource with Specifications.
        /// This methods is used inside <see cref="GetAsync(FilterSpecification{TEntity}, PagingSpecification, SortSpecification{TEntity})"/> implementations.
        /// Since this code is always the same for all the repositories, it was realized inside this class and made as protected.
        /// </summary>
        /// <param name="filterSpecification"></param>
        /// <param name="pagingSpecification"></param>
        /// <param name="sortSpecification"></param>
        /// <returns></returns>
        protected IQueryable<TEntity> GenerateQuery(FilterSpecification<TEntity> filterSpecification = null,
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
            return query;
        }

        /// <inheritdoc />
        public virtual async Task<TEntity> GetAsync(int id)
        {
            var result = await Context.Set<TEntity>().FindAsync(id);
            if (result == null)
                throw new IndexOutOfRangeException("Element doesn't exist.");
            return result;
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        private static Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> SortThenBy(Sort<TEntity> sortElement,
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

        private static Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> SortOrderBy(Sort<TEntity> sortElement)
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

        private static IQueryable<TEntity> SortQuery(SortSpecification<TEntity> sortSpecification, IQueryable<TEntity> query)
        {
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> sort = null;

            foreach (var sortElement in sortSpecification.SortElements)
            {
                sort = sort != null ? SortThenBy(sortElement, sort) : SortOrderBy(sortElement);
            }

            if (sort != null)
                query = sort(query);
            return query;
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<TEntity>> GetAsync(FilterSpecification<TEntity> filterSpecification = null, 
            PagingSpecification pagingSpecification = null,
            SortSpecification<TEntity> sortSpecification = null)
        {

            var query = GenerateQuery(filterSpecification, pagingSpecification, sortSpecification);
            return await query.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<int> CountWhereAsync(FilterSpecification<TEntity> filterSpecification = null)
        {
            var totalEntities = 0;
            IQueryable<TEntity> query = Context.Set<TEntity>();
            if (filterSpecification != null)
                query = query.Where(filterSpecification);
            if (query != null)
                totalEntities = await query.CountAsync();
            return totalEntities;
        }

        /// <inheritdoc />
        public virtual async Task<TEntity> AddAsync(TEntity entity)
        { 
            return (await Context.Set<TEntity>().AddAsync(entity)).Entity;
        }

        /// <inheritdoc />
        public virtual Task RemoveAsync(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public virtual Task RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<int> CountWhereAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).CountAsync();
        }

        /// <inheritdoc />
        public Task<int> CountAllAsync()
        {
            return Context.Set<TEntity>().CountAsync();
        }

        /// <inheritdoc />
        public virtual TEntity Get(int id)
        {
            var result = Context.Set<TEntity>().Find(id);
            if (result == null)
                throw new IndexOutOfRangeException("Element doesn't exist.");
            return result;
        }

        /// <inheritdoc />
        public virtual IEnumerable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().ToList();
        }

        /// <inheritdoc />
        public virtual TEntity Add(TEntity entity)
        {
            return Context.Set<TEntity>().Add(entity).Entity;
        }

        /// <inheritdoc />
        public virtual void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        /// <inheritdoc />
        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }

        /// <inheritdoc />
        public int CountWhere(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).Count();
        }

        /// <inheritdoc />
        public int CountAll()
        {
            return Context.Set<TEntity>().Count();
        }
    }
}
