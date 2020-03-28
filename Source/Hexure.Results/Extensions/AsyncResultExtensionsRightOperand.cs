using System;
using System.Collections.Generic;
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

        public static async Task<Result<T>> OnSuccess<T>(this Result result, Func<Task<T>> func)
        {
            if (result.IsFailure)
                return Result.Fail<T>(result.Error.ToArray());

            T value = await func().ConfigureAwait(Result.DefaultConfigureAwait);

            return Result.Ok(value);
        }

        public static async Task<Result<K>> OnSuccess<T, K>(this Result<T> result, Func<T, Task<Result<K>>> func)
        {
            if (result.IsFailure)
                return Result.Fail<K>(result.Error.ToArray());

            return await func(result.Value).ConfigureAwait(Result.DefaultConfigureAwait);
        }

        public static async Task<Result<T>> OnSuccess<T>(this Result result, Func<Task<Result<T>>> func)
        {
            if (result.IsFailure)
                return Result.Fail<T>(result.Error.ToArray());

            return await func().ConfigureAwait(Result.DefaultConfigureAwait);
        }

        public static async Task<Result<K>> OnSuccess<T, K>(this Result<T> result, Func<Task<Result<K>>> func)
        {
            if (result.IsFailure)
                return Result.Fail<K>(result.Error.ToArray());

            return await func().ConfigureAwait(Result.DefaultConfigureAwait);
        }

        public static async Task<Result> OnSuccess<T>(this Result<T> result, Func<T, Task<Result>> func)
        {
            if (result.IsFailure)
                return Result.Fail(result.Error.ToArray());

            return await func(result.Value).ConfigureAwait(Result.DefaultConfigureAwait);
        }

        public static async Task<Result> OnSuccess(this Result result, Func<Task<Result>> func)
        {
            if (result.IsFailure)
                return result;

            return await func().ConfigureAwait(Result.DefaultConfigureAwait);
        }

        public static async Task<Result<T>> Ensure<T>(this Result<T> result, Func<T, Task<bool>> predicate, Error error)
        {
            if (result.IsFailure)
                return result;

            if (!await predicate(result.Value).ConfigureAwait(Result.DefaultConfigureAwait))
                return Result.Fail<T>(error);

            return result;
        }

        public static async Task<Result> Ensure(this Result result, Func<Task<bool>> predicate, Error error)
        {
            if (result.IsFailure)
                return result;

            if (!await predicate().ConfigureAwait(Result.DefaultConfigureAwait))
                return Result.Fail(error);

            return result;
        }

        public static Task<Result<K>> Map<T, K>(this Result<T> result, Func<T, Task<K>> func)
            => result.OnSuccess(func);

        public static Task<Result<T>> Map<T>(this Result result, Func<Task<T>> func)
            => result.OnSuccess(func);

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

        public static async Task<T> OnBoth<T>(this Result result, Func<Result, Task<T>> func)
        {
            return await func(result).ConfigureAwait(Result.DefaultConfigureAwait);
        }

        public static async Task<K> OnBoth<T, K>(this Result<T> result, Func<Result<T>, Task<K>> func)
        {
            return await func(result).ConfigureAwait(Result.DefaultConfigureAwait);
        }

        public static async Task<Result<T>> OnFailure<T>(this Result<T> result, Func<Task> func)
        {
            if (result.IsFailure)
            {
                await func().ConfigureAwait(Result.DefaultConfigureAwait);
            }

            return result;
        }

        public static async Task<Result> OnFailure(this Result result, Func<Task> func)
        {
            if (result.IsFailure)
            {
                await func().ConfigureAwait(Result.DefaultConfigureAwait);
            }

            return result;
        }

        public static async Task<Result<T>> OnFailure<T>(this Result<T> result, Func<IReadOnlyList<Error>, Task> func)
        {
            if (result.IsFailure)
            {
                await func(result.Error).ConfigureAwait(Result.DefaultConfigureAwait);
            }

            return result;
        }

        public static async Task<Result<T>> OnFailureCompensate<T>(this Result<T> result, Func<Task<Result<T>>> func)
        {
            if (result.IsFailure)
                return await func().ConfigureAwait(Result.DefaultConfigureAwait);

            return result;
        }

        public static async Task<Result> OnFailureCompensate(this Result result, Func<Task<Result>> func)
        {
            if (result.IsFailure)
                return await func().ConfigureAwait(Result.DefaultConfigureAwait);

            return result;
        }

        public static async Task<Result<T>> OnFailureCompensate<T>(this Result<T> result, Func<IReadOnlyList<Error>, Task<Result<T>>> func)
        {
            if (result.IsFailure)
                return await func(result.Error).ConfigureAwait(Result.DefaultConfigureAwait);

            return result;
        }

        public static async Task<Result> Combine(this IEnumerable<Task<Result>> tasks)
        {
#if NET40
            Result[] results = await TaskEx.WhenAll(tasks).ConfigureAwait(Result.DefaultConfigureAwait);
#else
            Result[] results = await Task.WhenAll(tasks).ConfigureAwait(Result.DefaultConfigureAwait);
#endif
            return results.Combine();
        }

        public static async Task<Result<IEnumerable<T>>> Combine<T>(this IEnumerable<Task<Result<T>>> tasks
            )
        {
#if NET40
            Result<T>[] results = await TaskEx.WhenAll(tasks).ConfigureAwait(Result.DefaultConfigureAwait);
#else
            Result<T>[] results = await Task.WhenAll(tasks).ConfigureAwait(Result.DefaultConfigureAwait);
#endif
            return results.Combine();
        }

        public static async Task<Result> Combine(this Task<IEnumerable<Result>> task)
        {
            IEnumerable<Result> results = await task.ConfigureAwait(Result.DefaultConfigureAwait);
            return results.Combine();
        }

        public static async Task<Result<IEnumerable<T>>> Combine<T>(this Task<IEnumerable<Result<T>>> task)
        {
            IEnumerable<Result<T>> results = await task.ConfigureAwait(Result.DefaultConfigureAwait);
            return results.Combine();
        }

        public static async Task<Result> Combine(this Task<IEnumerable<Task<Result>>> task
            )
        {
            var tasks = await task.ConfigureAwait(Result.DefaultConfigureAwait);

#if NET40
            var results = await TaskEx.WhenAll(tasks).ConfigureAwait(Result.DefaultConfigureAwait);
#else
            var results = await Task.WhenAll(tasks).ConfigureAwait(Result.DefaultConfigureAwait);
#endif

            return results.Combine();
        }

        public static async Task<Result<IEnumerable<T>>> Combine<T>(this Task<IEnumerable<Task<Result<T>>>> task
            )
        {
            var tasks = await task.ConfigureAwait(Result.DefaultConfigureAwait);
#if NET40
            var results = await TaskEx.WhenAll(tasks).ConfigureAwait(Result.DefaultConfigureAwait);
#else
            var results = await Task.WhenAll(tasks).ConfigureAwait(Result.DefaultConfigureAwait);
#endif

            return results.Combine();
        }

        public static async Task<Result<TNew>> Combine<T, TNew>(this IEnumerable<Task<Result<T>>> tasks,
            Func<IEnumerable<T>, TNew> composer
            )
        {
#if NET40
            IEnumerable<Result<T>> results = await TaskEx.WhenAll(tasks).ConfigureAwait(Result.DefaultConfigureAwait);
#else
            IEnumerable<Result<T>> results = await Task.WhenAll(tasks).ConfigureAwait(Result.DefaultConfigureAwait);
#endif

            return results.Combine(composer);
        }

        public static async Task<Result<TNew>> Combine<T, TNew>(this Task<IEnumerable<Task<Result<T>>>> task,
            Func<IEnumerable<T>, TNew> composer
            )
        {
            IEnumerable<Task<Result<T>>> tasks = await task.ConfigureAwait(Result.DefaultConfigureAwait);
            return await tasks.Combine(composer);
        }

        public static async Task<Result> OnSuccessTry(this Task<Result> task, Action action,
            Func<Exception, Error> errorHandler = null)
        {
            var result = await task.ConfigureAwait(Result.DefaultConfigureAwait);
            return result.OnSuccessTry(action, errorHandler);
        }

        public static async Task<Result<T>> OnSuccessTry<T>(this Task<Result> task, Func<T> func,
            Func<Exception, Error> errorHandler = null)
        {
            var result = await task.ConfigureAwait(Result.DefaultConfigureAwait);
            return result.OnSuccessTry(func, errorHandler);
        }

        public static async Task<Result> OnSuccessTry<T>(this Task<Result<T>> task, Action<T> action,
            Func<Exception, Error> errorHandler = null)
        {
            var result = await task.ConfigureAwait(Result.DefaultConfigureAwait);
            return result.OnSuccessTry(action, errorHandler);
        }

        public static async Task<Result<R>> OnSuccessTry<T, R>(this Task<Result<T>> task, Func<T, R> action,
            Func<Exception, Error> errorHandler = null)
        {
            var result = await task.ConfigureAwait(Result.DefaultConfigureAwait);
            return result.OnSuccessTry(action, errorHandler);
        }
    }
}