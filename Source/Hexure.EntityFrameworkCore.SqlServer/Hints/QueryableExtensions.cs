using System.Linq;

namespace Hexure.EntityFrameworkCore.SqlServer.Hints
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WithHint<T>(this IQueryable<T> set, string hint) where T : class
        {
            HintInterceptor.HintValue = hint;
            return set;
        }
    }
}