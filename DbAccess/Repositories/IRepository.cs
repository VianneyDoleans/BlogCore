using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DbAccess.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetAsync(int id);
        Task<IEnumerable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> AddAsync(TEntity entity);

        Task RemoveAsync(TEntity entity);
        Task RemoveRangeAsync(IEnumerable<TEntity> entities);

        //Task<int> CountWhereAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAllAsync();

        TEntity Get(int id);
        //IEnumerable<TEntity> GetWhere(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> GetAll();
        //IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        TEntity Add(TEntity entity);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        int CountWhere(Expression<Func<TEntity, bool>> predicate);
        int CountAll();
    }
}
