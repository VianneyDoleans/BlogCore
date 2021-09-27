using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DbAccess.Specifications;
using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.SortSpecification;

namespace DbAccess.Repositories
{
    /// <summary>
    /// Interface of all Repositories. It defines the available generics methods necessary to manipulate the Resources from the database (CRUD and more).
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
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
