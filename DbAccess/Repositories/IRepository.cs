using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DbAccess.Specifications;
using DbAccess.Specifications.SortSpecification;

namespace DbAccess.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetAsync(int id);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> GetAsync(FilterSpecification<TEntity> filterSpecification = null,
            PagingSpecification pagingSpecification = null, 
            SortSpecification<TEntity> sortSpecification = null);

        Task<TEntity> AddAsync(TEntity entity);

        Task RemoveAsync(TEntity entity);

        Task RemoveRangeAsync(IEnumerable<TEntity> entities);

        Task<int> CountAllAsync();

        TEntity Get(int id);

        IEnumerable<TEntity> GetAll();

        TEntity Add(TEntity entity);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        int CountWhere(Expression<Func<TEntity, bool>> predicate);
        int CountAll();
    }
}
