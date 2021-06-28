using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DbAccess.DataContext;
using DbAccess.Specifications;
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

        public async Task<IEnumerable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return Context.Set<TEntity>();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync<TProperty>(FilterSpecification<TEntity> filterSpecification = null,
            PagingSpecification pagingSpecification = null,
            OrderBySpecification<TEntity, TProperty> odBySpecification = null,
            SortingDirectionSpecification sortingDirectionSpecification = SortingDirectionSpecification.Ascending)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();
            if (filterSpecification != null)
                query = query.Where(filterSpecification);

            if (pagingSpecification != null)
                query = query.Skip(pagingSpecification.Skip).Take(pagingSpecification.Take);

            if (odBySpecification != null && sortingDirectionSpecification == SortingDirectionSpecification.Ascending)
                query = query.OrderBy(odBySpecification.Order);

            if (odBySpecification != null && sortingDirectionSpecification == SortingDirectionSpecification.Descending)
                query = query.OrderByDescending(odBySpecification.Order);

            return await query.ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        { 
            return (await Context.Set<TEntity>().AddAsync(entity)).Entity;
        }

        public virtual async Task RemoveAsync(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public virtual async Task RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
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

        public IEnumerable<TEntity> GetWhere(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).ToList();
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return Context.Set<TEntity>();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).ToList();
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
