using System.Linq;

namespace Interfaces.Core.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Page<T>(this IQueryable<T> query, int page, int pageSize)
        {
            return query
                .Skip(page*pageSize)
                .Take(pageSize);
        }
    }
}
