using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hexure.Results.Extensions
{
    /// <summary>
    /// Extentions for async operations where the task appears in the left operand only
    /// </summary>
    public static class AsyncResultExtensionsLeftOperand
    {
        public static async Task<Result<K>> OnSuccess<T, K>(this Task<Result<T>> resultTask, Func<T, K> func)
        {
            Result<T> result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);
            return result.OnSuccess(func);
        }

        public static async Task<Result<K>> OnSuccess<T, K>(this Task<Result<T>> resultTask, Func<T, Result<K>> func)
        {
            Result<T> result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);
            return result.OnSuccess(func);
        }

        public static async Task<Result> OnSuccess<T>(this Task<Result<T>> resultTask, Func<T, Result> func)
        {
            Result<T> result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);
            return result.OnSuccess(func);
        }
        
        public static async Task<Result<T>> OnSuccess<T>(this Task<Result<T>> resultTask, Action<T> action)
        {
            Result<T> result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);
            return result.OnSuccess(action);
        }

        public static async Task<Result> OnFailure(this Task<Result> resultTask, Action<IReadOnlyList<Error>> action)
        {
            Result result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);
            return result.OnFailure(action);
        }
    }
}