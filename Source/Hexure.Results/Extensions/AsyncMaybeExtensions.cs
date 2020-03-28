using System.Threading.Tasks;

namespace Hexure.Results.Extensions
{
    public static class AsyncMaybeExtensions
    {
        public static async Task<Result<T>> ToResult<T>(this Task<Maybe<T>> maybeTask, Error error) where T : class
        {
            Maybe<T> maybe = await maybeTask.ConfigureAwait(Result.DefaultConfigureAwait);
            return maybe.ToResult(error);
        }
    }
}