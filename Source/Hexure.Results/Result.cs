using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Hexure.Results.Extensions;

namespace Hexure.Results
{
    public interface IResult
    {
        bool IsFailure { get; }
        bool IsSuccess { get; }
    }

    internal class ResultCommonLogic<TError>
    {
        public bool IsFailure { get; }
        public bool IsSuccess => !IsFailure;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TError _error;

        public TError Error
        {
            [DebuggerStepThrough]
            get
            {
                if (IsSuccess)
                    throw new ResultSuccessException();

                return _error;
            }
            protected set => _error = value;
        }

        [DebuggerStepThrough]
        public ResultCommonLogic(bool isFailure, TError error)
        {
            if (isFailure)
            {
                if (error == null)
                    throw new ArgumentNullException(nameof(error), ResultMessages.ErrorObjectIsNotProvidedForFailure);
            }
            else
            {
                if (error != null)
                    throw new ArgumentException(ResultMessages.ErrorObjectIsProvidedForSuccess, nameof(error));
            }

            IsFailure = isFailure;
            _error = error;
        }

        public void GetObjectData(SerializationInfo oInfo, StreamingContext oContext)
        {
            oInfo.AddValue("IsFailure", IsFailure);
            oInfo.AddValue("IsSuccess", IsSuccess);
            if (IsFailure)
            {
                oInfo.AddValue("Error", Error);
            }
        }
    }

    internal sealed class ResultCommonLogic : ResultCommonLogic<IReadOnlyList<Error>>
    {
        [DebuggerStepThrough]
        public static ResultCommonLogic Create(bool isFailure, params Error[] error)
        {
            error = error?.Where(x => x != null).ToArray();
            if (isFailure)
            {
                if (error.IsNullOrEmpty())
                    throw new ArgumentNullException(nameof(error), ResultMessages.ErrorMessageIsNotProvidedForFailure);
            }
            else
            {
                if (!error.IsNullOrEmpty())
                    throw new ArgumentException(ResultMessages.ErrorMessageIsProvidedForSuccess, nameof(error));
            }

            return new ResultCommonLogic(isFailure, error);
        }

        public ResultCommonLogic(bool isFailure, params Error[] error) : base(isFailure, error)
        {
        }

        public bool Violates(Error.ErrorType invariant)
        {
            if (IsSuccess)
                return false;
            return Error.Any(x => x.Code == invariant.Code);
        }

        public bool ViolatesOnly(Error.ErrorType invariant)
        {
            if (IsSuccess)
                return false;
            return Error.All(x => x.Code == invariant.Code);
        }

        public bool ViolatesOnly(Error.ErrorType invariant, int count)
        {
            if (IsSuccess)
                return false;
            return Error.Count == count && ViolatesOnly(invariant);
        }

        public bool HasError(Error error)
        {
            if (IsFailure)
                return Error.Any(x => x == error);

            return false;
        }

        public void SetPropertyName(string propertyName, Func<Error, bool> selector)
        {
            if (IsSuccess)
                return;

            Error = GetReplacedErrors();

            List<Error> GetReplacedErrors()
            {
                var result = new List<Error>();
                foreach (var error in Error)
                {
                    result.Add(selector(error) ? error.SetPropertyName(propertyName) : error);
                }

                return result;
            }
        }

        private string GetPropertyName<TModel>(Expression<Func<TModel, object>> expr, string separator = ".")
        {
            MemberExpression me;
            switch (expr.Body.NodeType)
            {
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    var ue = expr.Body as UnaryExpression;
                    me = (ue?.Operand) as MemberExpression;
                    break;

                default:
                    me = expr.Body as MemberExpression;
                    break;
            }

            var propertyNames = new List<string>();
            while (me != null)
            {
                string propertyName = me.Member.Name;
                propertyNames.Add(propertyName);

                me = me.Expression as MemberExpression;
            }
            propertyNames.Reverse();
            return string.Join(separator, propertyNames);
        }
    }

    public struct Result : IResult, ISerializable
    {
        private static readonly Result OkResult = new Result(false, null);

        public static bool DefaultConfigureAwait = false;
        public static string ErrorMessagesSeparator = ", ";

        void ISerializable.GetObjectData(SerializationInfo oInfo, StreamingContext oContext)
        {
            _logic.GetObjectData(oInfo, oContext);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ResultCommonLogic _logic;

        public bool IsFailure => _logic.IsFailure;
        public bool IsSuccess => _logic.IsSuccess;
        public IReadOnlyList<Error> Error => _logic.Error;

        [DebuggerStepThrough]
        private Result(bool isFailure, params Error[] error)
        {
            _logic = ResultCommonLogic.Create(isFailure, error);
        }

        [DebuggerStepThrough]
        public static Result Ok()
        {
            return OkResult;
        }

        [DebuggerStepThrough]
        public static Result Fail(Error error)
        {
            return new Result(true, error);
        }

        [DebuggerStepThrough]
        public static Result Fail(Error[] error)
        {
            return new Result(true, error);
        }

        [DebuggerStepThrough]
        public static Result Create(bool isSuccess, Error error)
        {
            return isSuccess
                ? Ok()
                : Fail(error);
        }

        public static Result<Maybe<T>> CreateOptional<T>(bool isEmpty, Func<Result<T>> createResult)
        {
            return isEmpty ? Ok(Maybe<T>.None) : createResult().OnSuccess(Maybe<T>.From);
        }

        public static Result Create(Func<bool> predicate, Error error)
        {
            return Create(predicate(), error);
        }

        public static async Task<Result> Create(Func<Task<bool>> predicate, Error error)
        {
            bool isSuccess = await predicate().ConfigureAwait(Result.DefaultConfigureAwait);
            return Create(isSuccess, error);
        }

        [DebuggerStepThrough]
        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(false, value, null);
        }

        [DebuggerStepThrough]
        public static Result<T> Fail<T>(Error error)
        {
            return new Result<T>(true, default(T), error);
        }

        [DebuggerStepThrough]
        public static Result<T> Fail<T>(Error[] error)
        {
            return new Result<T>(true, default(T), error);
        }

        public static Result<T> Create<T>(bool isSuccess, T value, Error error)
        {
            return isSuccess
                ? Ok(value)
                : Fail<T>(error);
        }

        public static Result<T> Create<T>(Func<bool> predicate, T value, Error error)
        {
            return Create(predicate(), value, error);
        }

        public static async Task<Result<T>> Create<T>(Func<Task<bool>> predicate, T value, Error error)
        {
            bool isSuccess = await predicate().ConfigureAwait(Result.DefaultConfigureAwait);
            return Create(isSuccess, value, error);
        }

        [DebuggerStepThrough]
        public static Result Combine(params Result[] results)
        {
            List<Result> failedResults = results.Where(x => x.IsFailure).ToList();

            if (!failedResults.Any())
                return Ok();

            Error[] errors = failedResults.SelectMany(x => x.Error).ToArray();
            return Fail(errors);
        }

        public bool Violates(Error.ErrorType invariant) => _logic.Violates(invariant);

        public bool ViolatesOnly(Error.ErrorType invariant) => _logic.ViolatesOnly(invariant);

        public bool ViolatesOnly(Error.ErrorType invariant, int count) => _logic.ViolatesOnly(invariant, count);

        public bool HasError(Error error) => _logic.HasError(error);
    }

    public struct Result<T> : IResult, ISerializable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ResultCommonLogic _logic;

        public bool IsFailure => _logic.IsFailure;
        public bool IsSuccess => _logic.IsSuccess;
        public IReadOnlyList<Error> Error => _logic.Error;

        void ISerializable.GetObjectData(SerializationInfo oInfo, StreamingContext oContext)
        {
            _logic.GetObjectData(oInfo, oContext);

            if (IsSuccess)
            {
                oInfo.AddValue("Value", Value);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly T _value;

        public T Value
        {
            [DebuggerStepThrough]
            get
            {
                if (!IsSuccess)
                    throw new ResultFailureException(Error.GetErrors());

                return _value;
            }
        }
        
        public static implicit operator Result(Result<T> result)
        {
            if (result.IsSuccess)
                return Result.Ok();
            else
                return Result.Fail(result.Error.ToArray());
        }

        [DebuggerStepThrough]
        internal Result(bool isFailure, T value, params Error[] error)
        {
            _logic = ResultCommonLogic.Create(isFailure, error);
            _value = value;
        }

        public bool Violates(Error.ErrorType invariant) => _logic.Violates(invariant);

        public bool ViolatesOnly(Error.ErrorType invariant) => _logic.ViolatesOnly(invariant);

        public bool ViolatesOnly(Error.ErrorType invariant, int count) => _logic.ViolatesOnly(invariant, count);

        public bool HasError(Error error) => _logic.HasError(error);
    }
}