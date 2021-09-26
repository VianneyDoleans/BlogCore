using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MyBlogAPI.Extensions
{
    public static class ControllerParameters
    {
        public static async /*Task<PagedBlogResponse<TModel>>*/ Task<IEnumerable<TModel>> PaginateAsync<TModel>(
            this IQueryable<TModel> query,
            int offset,
            int limit,
            CancellationToken cancellationToken)
            where TModel : class
        {
            //offset = (offset < 0) ? 0 : offset;
            //offset = (offset > 100) ? 100 : offset;

            //var totalItemsCountTask = query.CountAsync(cancellationToken);

            //var startRow = (offset - 1) * limit;
            IList<TModel> data = await query
                .Skip(offset)
                .Take(limit)
                .ToListAsync(cancellationToken);

            //paged.TotalPages = (int) Math.Ceiling(paged.TotalItems / (double) limit);


            //var paged = new PagedBlogResponse<TModel>(data, offset, limit, await totalItemsCountTask);
            return data; //paged;
        }
    }
}
