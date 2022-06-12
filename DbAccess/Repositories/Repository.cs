using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBAccess.DataContext;
using DBAccess.Specifications;
using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.SortSpecification;
using Microsoft.EntityFrameworkCore;

namespace DBAccess.Repositories
{
    /// <summary>
    /// Implementation of <see cref="IRepository{TEntity}"/> interface.
    /// It implements the available generics methods necessary to manipulate the Resources from the database (CRUD and more).
    /// It also defines some protected methods needed for Repositories class which inherited from <see cref="Repository{TEntity}"/>.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly BlogCoreContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class.
        /// </summary>
        /// <param name="context"></param>
        public Repository(BlogCoreContext context)
        {
            _context = context;
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
            IQueryable<TEntity> query = _context.Set<TEntity>();
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
            var result = await _context.Set<TEntity>().FindAsync(id);
            if (result == null)
                throw new IndexOutOfRangeException("Element doesn't exist.");
            return result;
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
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
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
        public async Task<int> CountWhereAsync(FilterSpecification<TEntity> filterSpecification = null)
        {
            var totalEntities = 0;
            IQueryable<TEntity> query = _context.Set<TEntity>();
            if (filterSpecification != null)
                query = query.Where(filterSpecification);
            if (query != null)
                totalEntities = await query.CountAsync();
            return totalEntities;
        }

        /// <inheritdoc />
        public virtual async Task<TEntity> AddAsync(TEntity entity)
        { 
            return (await _context.Set<TEntity>().AddAsync(entity)).Entity;
        }

        /// <inheritdoc />
        public virtual Task RemoveAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public virtual Task RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            _context.Set<TEntity>().RemoveRange(entities);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<int> CountAllAsync()
        {
            return _context.Set<TEntity>().CountAsync();
        }

        /// <inheritdoc />
        public virtual TEntity Get(int id)
        {
            var result = _context.Set<TEntity>().Find(id);
            if (result == null)
                throw new IndexOutOfRangeException("Element doesn't exist.");
            return result;
        }

        /// <inheritdoc />
        public virtual IEnumerable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().ToList();
        }

        /// <inheritdoc />
        public virtual TEntity Add(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            return _context.Set<TEntity>().Add(entity).Entity;
        }

        /// <inheritdoc />
        public virtual void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        /// <inheritdoc />
        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }

        /// <inheritdoc />
        public int CountAll()
        {
            return _context.Set<TEntity>().Count();
        }
    }
}
