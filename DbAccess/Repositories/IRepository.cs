using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DbAccess.Specifications;

namespace DbAccess.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetAsync(int id);
        Task<IEnumerable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> GetAsync<TProperty>(FilterSpecification<TEntity> filterSpecification = null,
            PagingSpecification pagingSpecification = null,
            OrderBySpecification<TEntity, TProperty> odBySpecification = null,
            SortingDirectionSpecification sortingDirectionSpecification = SortingDirectionSpecification.Ascending);

        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> AddAsync(TEntity entity);

        Task RemoveAsync(TEntity entity);
        Task RemoveRangeAsync(IEnumerable<TEntity> entities);

        Task<int> CountAllAsync();

        TEntity Get(int id);

        IQueryable<TEntity> GetAll();

        TEntity Add(TEntity entity);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        int CountWhere(Expression<Func<TEntity, bool>> predicate);
        int CountAll();
    }
}
