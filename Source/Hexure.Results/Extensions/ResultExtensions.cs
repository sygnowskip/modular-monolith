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

        public static Result AndEnsure(this Result result, Func<bool> predicate, Error errorMessage)
        {
            if (result.IsFailure)
                return result;

            if (!predicate())
                return Result.Combine(result, Result.Fail(errorMessage));

            return result;
        }
        
        public static Result<T> BindErrorsTo<T>(this Result<T> result, string property)
        {
            return result
                .OnFailure(errors =>
                {
                    foreach (var error in errors)
                    {
                        error.SetPropertyName(property.ToCamelCase());
                    }
                });
        }
    }
}