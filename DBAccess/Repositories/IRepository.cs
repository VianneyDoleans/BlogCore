using System.Collections.Generic;
using System.Threading.Tasks;
using DBAccess.Specifications;
using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.SortSpecification;

namespace DBAccess.Repositories
{
    /// <summary>
    /// Interface of all Repositories. It defines the available generics methods necessary to manipulate the Resources from the database (CRUD and more).
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Method used to get a <see cref="TEntity"/> resource by giving its Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetAsync(int id);

        /// <summary>
        /// Method used to get <see cref="TEntity"/> resource(s) by specified filter(s), pagination, and sort(s) (orderBy, etc.).
        /// </summary>
        /// <param name="filterSpecification"></param>
        /// <param name="pagingSpecification"></param>
        /// <param name="sortSpecification"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAsync(FilterSpecification<TEntity> filterSpecification = null,
            PagingSpecification pagingSpecification = null,
            SortSpecification<TEntity> sortSpecification = null);

        /// <summary>
        /// Method used to get all <see cref="TEntity"/> resources.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Method used to count <see cref="TEntity"/> resources where the expression match.
        /// </summary>
        /// <param name="filterSpecification"></param>
        /// <returns></returns>
        Task<int> CountWhereAsync(FilterSpecification<TEntity> filterSpecification = null);

        /// <summary>
        /// Method used to add a <see cref="TEntity"/> resource.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity> AddAsync(TEntity entity);

        /// <summary>
        /// Method used to remove a <see cref="TEntity"/> resource.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task RemoveAsync(TEntity entity);

        /// <summary>
        /// Method used to remove a list of <see cref="TEntity"/> resources.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task RemoveRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Method used to count all existing <see cref="TEntity"/>
        /// </summary>
        /// <returns></returns>
        Task<int> CountAllAsync();

        /// <summary>
        /// Method used to get a <see cref="TEntity"/> resource by giving its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity Get(int id);

        /// <summary>
        /// Method used to get all existing <see cref="TEntity"/> resources.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// Method used to add a <see cref="TEntity"/> resource.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Add(TEntity entity);

        /// <summary>
        /// Method used to remove a <see cref="TEntity"/> resource.
        /// </summary>
        /// <param name="entity"></param>
        void Remove(TEntity entity);

        /// <summary>
        /// Method used to remove a list of <see cref="TEntity"/> resources.
        /// </summary>
        /// <param name="entities"></param>
        void RemoveRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Method used to count all existing <see cref="TEntity"/>
        /// </summary>
        /// <returns></returns>
        int CountAll();
    }
}
