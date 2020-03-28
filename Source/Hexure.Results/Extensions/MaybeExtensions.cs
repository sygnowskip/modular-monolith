namespace Hexure.Results.Extensions
{
    public static class MaybeExtensions
    {
        public static Result<T> ToResult<T>(this Maybe<T> maybe, Error error)
        {
            if (maybe.HasNoValue)
                return Result.Fail<T>(error);

            return Result.Ok(maybe.Value);
        }
    }
}