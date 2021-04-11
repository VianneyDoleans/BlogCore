using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyBlogAPI.Responses;

namespace MyBlogAPI.Extensions
{
    public static class DataPagerExtension
    {
        public static async Task<PagedBlogResponse<TModel>> PaginateAsync<TModel>(
            this IQueryable<TModel> query,
            int page,
            int limit,
            CancellationToken cancellationToken)
            where TModel : class
        {
            page = (page < 0) ? 1 : page;

            var totalItemsCountTask = query.CountAsync(cancellationToken);

            var startRow = (page - 1) * limit;
            IList<TModel> data = await query
                .Skip(startRow)
                .Take(limit)
                .ToListAsync(cancellationToken);

            //paged.TotalPages = (int) Math.Ceiling(paged.TotalItems / (double) limit);


            var paged = new PagedBlogResponse<TModel>(data, page, limit, await totalItemsCountTask);
            return paged;
        }
    }
}
