using System.Collections.Generic;
using System.Linq;

namespace Hexure.Results.Extensions
{
    public static class ErrorExtensions
    {
        public static string GetErrors(this IEnumerable<Error> errors) => GetErrors(errors, Result.ErrorMessagesSeparator);

        public static string GetErrors(this IEnumerable<Error> errors, string separator)
        {
            separator = separator ?? Result.ErrorMessagesSeparator;
            return string.Join(separator, errors.Select(x => x.Message));
        }
    }
}