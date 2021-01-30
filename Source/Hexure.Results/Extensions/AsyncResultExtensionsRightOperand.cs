using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hexure.Results.Extensions
{
    /// <summary>
    ///     Extentions for async operations where the task appears in the right operand only
    /// </summary>
    public static class AsyncResultExtensionsRightOperand
    {
        public static async Task<Result<K>> OnSuccess<T, K>(this Result<T> result, Func<T, Task<K>> func)
        {
            if (result.IsFailure)
                return Result.Fail<K>(result.Error.ToArray());

            K value = await func(result.Value).ConfigureAwait(Result.DefaultConfigureAwait);

            return Result.Ok(value);
        }

        public static async Task<Result<K>> OnSuccess<T, K>(this Result<T> result, Func<T, Task<Result<K>>> func)
        {
            if (result.IsFailure)
                return Result.Fail<K>(result.Error.ToArray());

            return await func(result.Value).ConfigureAwait(Result.DefaultConfigureAwait);
        }

        public static async Task<Result> OnSuccess<T>(this Result<T> result, Func<T, Task<Result>> func)
        {
            if (result.IsFailure)
                return Result.Fail(result.Error.ToArray());

            return await func(result.Value).ConfigureAwait(Result.DefaultConfigureAwait);
        }
        
        public static async Task<Result<T>> OnSuccess<T>(this Result<T> result, Func<T, Task> action)
        {
            if (result.IsSuccess)
            {
                await action(result.Value).ConfigureAwait(Result.DefaultConfigureAwait);
            }

            return result;
        }

        public static async Task<Result> OnSuccess(this Result result, Func<Task> action)
        {
            if (result.IsSuccess)
            {
                await action().ConfigureAwait(Result.DefaultConfigureAwait);
            }

            return result;
        }
    }
}