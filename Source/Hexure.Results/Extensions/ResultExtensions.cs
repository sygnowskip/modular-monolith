using System;
using System.Collections.Generic;
using System.Linq;

namespace Hexure.Results.Extensions
{
    public static partial class ResultExtensions
    {
        public static Result<K> OnSuccess<T, K>(this Result<T> result, Func<T, K> func)
        {
            if (result.IsFailure)
                return Result.Fail<K>(result.Error.ToArray());

            return Result.Ok(func(result.Value));
        }

        public static Result<T> OnSuccess<T>(this Result result, Func<T> func)
        {
            if (result.IsFailure)
                return Result.Fail<T>(result.Error.ToArray());

            return Result.Ok(func());
        }

        public static Result<K> OnSuccess<T, K>(this Result<T> result, Func<T, Result<K>> func)
        {
            if (result.IsFailure)
                return Result.Fail<K>(result.Error.ToArray());

            return func(result.Value);
        }

        public static Result<T> OnSuccess<T>(this Result result, Func<Result<T>> func)
        {
            if (result.IsFailure)
                return Result.Fail<T>(result.Error.ToArray());

            return func();
        }

        public static Result<K> OnSuccess<T, K>(this Result<T> result, Func<Result<K>> func)
        {
            if (result.IsFailure)
                return Result.Fail<K>(result.Error.ToArray());

            return func();
        }

        public static Result OnSuccess<T>(this Result<T> result, Func<T, Result> func)
        {
            if (result.IsFailure)
                return Result.Fail(result.Error.ToArray());

            return func(result.Value);
        }

        public static Result OnSuccess(this Result result, Func<Result> func)
        {
            if (result.IsFailure)
                return result;

            return func();
        }

        public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> predicate, Error error)
        {
            if (result.IsFailure)
                return result;

            if (!predicate(result.Value))
                return Result.Fail<T>(error);

            return result;
        }

        public static Result Ensure(this Result result, Func<bool> predicate, Error error)
        {
            if (result.IsFailure)
                return result;

            if (!predicate())
                return Result.Fail(error);

            return result;
        }

        public static Result<K> Map<T, K>(this Result<T> result, Func<T, K> func)
            => result.OnSuccess(func);

        public static Result<T> Map<T>(this Result result, Func<T> func)
            => result.OnSuccess(func);

        public static Result<T> OnSuccess<T>(this Result<T> result, Action<T> action)
        {
            if (result.IsSuccess)
            {
                action(result.Value);
            }

            return result;
        }

        public static Result OnSuccess(this Result result, Action action)
        {
            if (result.IsSuccess)
            {
                action();
            }

            return result;
        }

        public static Result OnSuccessTry(this Result result, Action action, Func<Exception, Error> errorHandler)
        {
            return result.IsFailure
                ? result
                : Result.Try(action, errorHandler);
        }

        public static Result<T> OnSuccessTry<T>(this Result result, Func<T> func, Func<Exception, Error> errorHandler)
        {
            return result.IsFailure
                ? Result.Fail<T>(result.Error.ToArray())
                : Result.Try(func, errorHandler);
        }

        public static Result OnSuccessTry<T>(this Result<T> result, Action<T> action, Func<Exception, Error> errorHandler)
        {
            return result.IsFailure
                ? Result.Fail(result.Error.ToArray())
                : Result.Try(() => action(result.Value), errorHandler);
        }

        public static Result<R> OnSuccessTry<T, R>(this Result<T> result, Func<T, R> func, Func<Exception, Error> errorHandler)
        {
            return result.IsFailure
                ? Result.Fail<R>(result.Error.ToArray())
                : Result.Try(() => func(result.Value), errorHandler);
        }

        public static T OnBoth<T>(this Result result, Func<Result, T> func)
        {
            return func(result);
        }

        public static K OnBoth<T, K>(this Result<T> result, Func<Result<T>, K> func)
        {
            return func(result);
        }

        public static Result<T> OnFailure<T>(this Result<T> result, Action action)
        {
            if (result.IsFailure)
            {
                action();
            }

            return result;
        }

        public static Result OnFailure(this Result result, Action action)
        {
            if (result.IsFailure)
            {
                action();
            }

            return result;
        }

        public static Result<T> OnFailure<T>(this Result<T> result, Action<IReadOnlyList<Error>> action)
        {
            if (result.IsFailure)
            {
                action(result.Error);
            }

            return result;
        }

        public static Result OnFailure(this Result result, Action<IReadOnlyList<Error>> action)
        {
            if (result.IsFailure)
            {
                action(result.Error);
            }

            return result;
        }

        public static Result<T> OnFailureCompensate<T>(this Result<T> result, Func<Result<T>> func)
        {
            if (result.IsFailure)
                return func();

            return result;
        }

        public static Result OnFailureCompensate(this Result result, Func<Result> func)
        {
            if (result.IsFailure)
                return func();

            return result;
        }

        public static Result<T> OnFailureCompensate<T>(this Result<T> result, Func<IReadOnlyList<Error>, Result<T>> func)
        {
            if (result.IsFailure)
                return func(result.Error);

            return result;
        }

        public static Result OnFailureCompensate(this Result result, Func<IReadOnlyList<Error>, Result> func)
        {
            if (result.IsFailure)
                return func(result.Error);

            return result;
        }

        public static Result Combine(this IEnumerable<Result> results)
        {
            return Result.Combine(results as Result[] ?? results.ToArray());
        }

        public static Result<IEnumerable<T>> Combine<T>(this IEnumerable<Result<T>> results)
        {
            var data = results as Result<T>[] ?? results.ToArray();

            var result = Result.Combine(data);

            return result.IsSuccess
                ? Result.Ok(data.Select(e => e.Value))
                : Result.Fail<IEnumerable<T>>(result.Error.ToArray());
        }

        public static Result<TNew> Combine<T, TNew>(this IEnumerable<Result<T>> results,
            Func<IEnumerable<T>, TNew> composer)
        {
            Result<IEnumerable<T>> result = results.Combine();

            return result.IsSuccess
                ? Result.Ok(composer(result.Value))
                : Result.Fail<TNew>(result.Error.ToArray());
        }

        public static Result AndEnsure(this Result result, Func<bool> predicate, Error errorMessage)
        {
            if (result.IsFailure)
                return result;

            if (!predicate())
                return Result.Combine(result, Result.Fail(errorMessage));

            return result;
        }

        public static Result AndEnsure(this Result result, bool condition, Error errorMessage)
        {
            if (result.IsFailure)
                return result;
            
            if (!condition)
                return Result.Combine(result, Result.Fail(errorMessage));

            return result;
        }

        public static Result Add(this Result result, Result other)
        {
            return Result.Combine(result, other);
        }

        public static Result<T> AddAndMap<T>(this Result result, Result<T> other)
        {
            if (result.IsFailure)
                return Result.Fail<T>(Result.Combine(result, other).Error.ToArray());

            return other;
        }

        public static Result Ensure(bool condition, Error errorMessage)
        {
            if (!condition)
                return Result.Fail(errorMessage);

            return Result.Ok();
        }

        public static Result AddError(this Result result, Error errorMessage)
            => Result.Combine(result, Result.Fail(errorMessage));

        /// <summary>
        /// Allows using <see cref="Result{T}"/> with LINQ query syntax.
        /// </summary>
        public static Result<R> SelectMany<T, K, R>(
            this Result<T> result,
            Func<T, Result<K>> bind,
            Func<T, K, R> @return) =>
            result.OnSuccess(x => bind(x)
                    .OnSuccess(y => Result.Ok(@return(x, y))));
    }
}